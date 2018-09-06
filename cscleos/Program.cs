using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PowerArgs;
using System;
using System.Collections.Generic;
using ServiceStack;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Linq;
using EOSNewYork.EOSCore;
using EOSNewYork.EOSCore.Response.Table;
using EOSNewYork.EOSCore.Response.API;
using EOSNewYork.EOSCore.Utilities;
using EOSNewYork.EOSCore.Lib;
using NLog;

namespace cscleos
{
    [TabCompletion]
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class GetProgram
    {
        static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static TableAPI tableAPI;
        
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Retrieve data from one of the well known EOS tables")]
        public void getKnownTable([ArgRequired]TableArguments args)
        {
            tableAPI = new TableAPI(args.host.AbsoluteUri);
            logger.Info("Using host: {0}", args.host);

            var fieldDoesNotExistError = false;
            ServiceStack.Text.CsvConfig.ItemSeperatorString = args.delimiter;
            switch (args.table)
            {
                case TableTypes.voters:
                    fieldDoesNotExistError = CheckProperties<VoterRow>(args);
                    if (!fieldDoesNotExistError)
                    {
                        List<VoterRow> voters = tableAPI.GetVoterRows();
                        FilterAndOutput(args, voters);
                    }
                    break;
                case TableTypes.producers:
                    fieldDoesNotExistError = CheckProperties<ProducerRow>(args);
                    if (!fieldDoesNotExistError)
                    {
                        List<ProducerRow> producers = tableAPI.GetProducerRows();
                        FilterAndOutput(args, producers);
                    }
                    break;
                case TableTypes.global:
                    fieldDoesNotExistError = CheckProperties<GlobalRow>(args);
                    if (!fieldDoesNotExistError)
                    {
                        List<GlobalRow> global = tableAPI.GetGlobalRows();
                        FilterAndOutput(args, global);
                    }
                    break;
                case TableTypes.namebids:
                    fieldDoesNotExistError = CheckProperties<NameBidsRow>(args);
                    if(!fieldDoesNotExistError)
                    {
                        List<NameBidsRow> namebids = tableAPI.GetNameBidRows();
                        FilterAndOutput(args, namebids);
                    }
                    break;
            }
        }

        private static bool CheckProperties<T>(TableArguments args) where T : IEOSTable
        {
            bool error = false;
            if(args.fieldList != null)
            {
                foreach (var field in args.fieldList)
                {
                    if (typeof(T).GetProperty(field) == null)
                    {
                        Console.WriteLine(string.Format("Table rows do not contain field \"{0}\"", field));
                        Console.WriteLine("Valid fields include:");
                        PropertyInfo[] properties = typeof(T).GetProperties();
                        foreach (var property in properties)
                        {
                            Console.WriteLine(property.Name);
                        }
                        error = true;
                    }
                }
            }
            return error;
        }

        private static void FilterAndOutput<T>(TableArguments args, List<T> queryResult) where T : IEOSTable
        {
            List<dynamic> filteredFieldObject = null;
            if (args.fieldList != null)
            {
                foreach (var field in args.fieldList)
                {
                    if(typeof(T).GetProperty(field) == null)
                    {
                        Console.WriteLine(string.Format("Table rows do not contain field \"{0}\"",field));
                        return;
                    }
                }               

                filteredFieldObject = EOSUtility.FilterFields(args.fieldList, queryResult);
            }
                
            if (args.outputFormat == OutputFormats.json)
            {
                string json = string.Empty;
                if (filteredFieldObject == null)
                    json = JsonConvert.SerializeObject(queryResult, Formatting.Indented);
                else
                    json = JsonConvert.SerializeObject(filteredFieldObject, Formatting.Indented);

                Console.WriteLine(json);
            }
            else
            {
                if (filteredFieldObject == null)
                {
                    Console.Write(queryResult.ToCsv());
                }
                else
                {
                    Console.Write(filteredFieldObject.ToCsv());
                }
            }
        }
    }

    public class TableArguments
    {
        [StickyArg]
        [ArgRequired, ArgDescription("URL of the API that will be queried. This is a sticky param and will be remembered after you've used it once."), ArgPosition(1)]
        public Uri host { get; set; }

        [ArgRequired, ArgDescription("The name of the table"), ArgPosition(2)]
        public TableTypes table { get; set; }

        [ArgDefaultValue("csv")]
        [ArgRequired, ArgDescription("The format of the result. Defaults to Tab Seperated"), ArgPosition(3)]
        public OutputFormats outputFormat { get; set; }

        [ArgDefaultValue("\t")]
        [ArgDescription("The characted to use as a delimiter when outputting as a CSV. Default is a tab."), ArgPosition(4)]
        public string delimiter { get; set; }

        [ArgDescription("A comma separated list of fields that should be returned. Default is to return all fields."), ArgPosition(5)]
        public List<string> fieldList { get; set; }
    }

    public enum OutputFormats
    {
        csv, json
    }

    public enum TableTypes
    {
        producers, voters, global, namebids
    }

    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<GetProgram>(args);
        }
    }
}
