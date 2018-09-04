using Newtonsoft.Json;
using PowerArgs;
using System;
using System.Collections.Generic;
using ServiceStack;
using System.Reflection;
using System.Text;
using System.Linq;
using EOSNewYork.EOSCore;
using EOSNewYork.EOSCore.Response.Table;
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
        static Uri HOST = new Uri("https://api.eosnewyork.io");

        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Retrieve data from one of the well known EOS tables")]
        public void getKnownTable([ArgRequired]getKnownTableArguments arg)
        {

            logger.Info("Using API: {0}", arg.host);

            var fieldDoesNotExistError = false;
            ServiceStack.Text.CsvConfig.ItemSeperatorString = arg.delimiter;
            switch (arg.table)
            {
                case TableTypes.voters:
                    fieldDoesNotExistError = CheckProperties<VoterRow>(arg);
                    if (!fieldDoesNotExistError)
                    {
                        List<VoterRow> voters = new EOS_Table<VoterRow>(arg.host).GetRowsFromAPIAsync().Result;
                        FilterAndOutput(arg, voters);
                    }
                    break;
                case TableTypes.producers:
                    fieldDoesNotExistError = CheckProperties<ProducerRow>(arg);
                    if (!fieldDoesNotExistError)
                    {
                        List<ProducerRow> producers = new EOS_Table<ProducerRow>(HOST).GetRowsFromAPIAsync().Result;
                        FilterAndOutput(arg, producers);
                    }
                    break;
                case TableTypes.global:
                    fieldDoesNotExistError = CheckProperties<GlobalRow>(arg);
                    if (!fieldDoesNotExistError)
                    {
                        List<GlobalRow> global = new EOS_Table<GlobalRow>(HOST).GetRowsFromAPIAsync().Result;
                        FilterAndOutput(arg, global);
                    }
                    break;
                case TableTypes.namebids:
                    fieldDoesNotExistError = CheckProperties<NameBidsRow>(arg);
                    if(!fieldDoesNotExistError)
                    {
                        List<NameBidsRow> namebids = new EOS_Table<NameBidsRow>(HOST).GetRowsFromAPIAsync().Result;
                        FilterAndOutput(arg, namebids);
                    }
                    break;
            }
        }

        private static bool CheckProperties<T>(getKnownTableArguments arg) where T : IEOSTable
        {
            bool error = false;
            if(arg.fieldList != null)
            {
                foreach (var field in arg.fieldList)
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

        private static void FilterAndOutput<T>(getKnownTableArguments arg, List<T> queryResult) where T : IEOSTable
        {
            List<dynamic> filteredFieldObject = null;
            if (arg.fieldList != null)
            {
                foreach (var field in arg.fieldList)
                {
                    if(typeof(T).GetProperty(field) == null)
                    {
                        Console.WriteLine(string.Format("Table rows do not contain field \"{0}\"",field));
                        return;
                    }
                }               

                filteredFieldObject = EOSUtility.FilterFields(arg.fieldList, queryResult);
            }
                
            if (arg.outputFormat == OutputFormats.json)
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

    public class getKnownTableArguments
    {
        [StickyArg]
        //[ArgDefaultValue("https://api.eosnewyork.io")]
        [ArgRequired, ArgDescription("URL of the API that will be queried. This is a sticky param and will be remembered after you've used it once."), ArgPosition(1)]
        public Uri host { get; set; }

        [ArgRequired, ArgDescription("The name of the table"), ArgPosition(2)]
        public TableTypes table { get; set; }

        [ArgDefaultValue("csv")]
        [ArgRequired, ArgDescription("The format of the result. Defaults to Tab Seperated"), ArgPosition(3)]
        public OutputFormats outputFormat { get; set; }

        [ArgDefaultValue("\t")]
        [ArgDescription("The characted to use as a delimiter when outputting as a CSV. Default is a tab."), ArgPosition(4)]
        public String delimiter { get; set; }

        [ArgDescription("A comma separated list of fields that should be returned. Default is to return all fields."), ArgPosition(5)]
        public List<String> fieldList { get; set; }
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
