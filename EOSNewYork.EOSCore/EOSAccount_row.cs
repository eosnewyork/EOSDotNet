using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSNewYork.EOSCore
{

    //{"account_name":"bob123451234","head_block_num":37112,"head_block_time":"2018-07-24T17:47:47.000","
    //privileged":false,"last_code_update":"2018-07-24T12:38:31.500","created":"2018-07-24T12:38:31.500","
    //core_liquid_balance":"99975.0000 EOS",
    //"ram_quota":1018841,
    //"net_weight":1000000000,
    //"cpu_weight":1000000000,
    //"net_limit":{"used":4401,"available":"56411925066","max":"56411929467"},
    //"cpu_limit":{"used":2000,"available":"10759719654","max":"10759721654"},
    //"ram_usage":39515,
    //"permissions":[
   /* 
    {"perm_name":"active",
    "parent":"owner",
    "required_auth":
        {"threshold":1,
            "keys":[{"key":"EOS89EDpYUcbrZauAcvAN78EbMzK4kfC7shhuTH4Dr2A3ZYanyCax","weight":1}],
            "accounts":[],
            "waits":[]}},{"perm_name":"owner","parent":"","required_auth":{"threshold":1,"keys":[{"key":"EOS89EDpYUcbrZauAcvAN78EbMzK4kfC7shhuTH4Dr2A3ZYanyCax","weight":1}],"accounts":[],"waits":[]}}],
            
            "total_resources":{"owner":"bob123451234","net_weight":"100000.0000 EOS","cpu_weight":"100000.0000 EOS","ram_bytes":1018841},
            "self_delegated_bandwidth":{"from":"bob123451234","to":"bob123451234","net_weight":"100000.0000 EOS","cpu_weight":"100000.0000 EOS"},
            "refund_request":null,           
            "voter_info":{"owner":"bob123451234","proxy":"","producers":[],"staked":2000000000,"last_vote_weight":"0.00000000000000000","proxied_vote_weight":"0.00000000000000000","is_proxy":0}}
*/
    public class EOSAccount_row : IEOAPI
    {
        public string account_name { get; set; }
        public Int64 head_block_num { get; set; }
        public String head_block_time { get; set; }
        public DateTime head_block_time_datetime
        {
            get
            {
                return DateTime.SpecifyKind((DateTime.Parse(head_block_time)), DateTimeKind.Utc);
            }
        }

        public bool privileged { get; set; }
        public String last_code_update { get; set; }
        public DateTime last_code_update_datetime { get
            {
                return DateTime.SpecifyKind((DateTime.Parse(last_code_update)), DateTimeKind.Utc);
            }
        }

        public String created { get; set; }
        public DateTime created_datetime
        {
            get
            {
                return DateTime.SpecifyKind((DateTime.Parse(created)), DateTimeKind.Utc);
            }
        }

        public String core_liquid_balance { get; set; }
        public decimal core_liquid_balance_ulong { get {
                var clean_core_liquid_balance = string.Empty;
                if (core_liquid_balance == null)
                    clean_core_liquid_balance = "0.0";
                else
                    clean_core_liquid_balance = core_liquid_balance.Trim().Replace(" EOS", "");

                return decimal.Parse(clean_core_liquid_balance);
            }
        }
        public Int64 ram_quota { get; set; }
        public Int64 ram_usage { get; set; }
        public Int64 net_weight { get; set; }
        public Int64 cpu_weight { get; set; }
        public EOSAccount_limit net_limit { get; set; }
        public EOSAccount_limit cpu_limit { get; set; }
        //permissions here
        public EOSAccount_total_resources total_resources { get; set; }
        public EOSAccount_self_delegated_bandwidth self_delegated_bandwidth { get; set; }
        public EOSAccount_refundRequest refund_request { get; set; }
        public EOSAccount_voter_info voter_info { get; set; }
        public List<EOSAccount_permission> permissions { get; set; }


        public class postData {
            public string account_name { get; set; }
        }

        public EOSAPIMetadata getMetadata()
        {
            var meta = new EOSAPIMetadata
            {
                uri = "v1/chain/get_account"
            };

            return meta;
        }
    }

    public class EOSAccount_limit
    {
        public Int64 used { get; set; }
        public Int64 available { get; set; }
        public Int64 max { get; set; }
    }

    public class EOSAccount_total_resources
    {
        public string owner { get; set; }
        public string net_weight { get; set; }
        public string cpu_weight { get; set; }
        public Int64 ram_bytes { get; set; }
    }

    public class EOSAccount_self_delegated_bandwidth
    {
        public string from { get; set; }
        public string to { get; set; }
        public string net_weight { get; set; }
        public string cpu_weight { get; set; }

        public decimal net_weight_decimal
        {
            get
            {
                string net_weight_clean = string.Empty;
                if (net_weight == null)
                    net_weight_clean = "0.0";
                else
                    net_weight_clean = net_weight.Trim().Replace(" EOS", "");

                return decimal.Parse(net_weight_clean);
            }
        }

        public decimal cpu_weight_decimal
        {
            get
            {
                string cpu_weight_clean = string.Empty;
                if (cpu_weight == null)
                    cpu_weight_clean = "0.0";
                else
                    cpu_weight_clean = cpu_weight.Trim().Replace(" EOS", "");

                return decimal.Parse(cpu_weight_clean);
            }
        }
    }

    public class EOSAccount_voter_info
    {
        public string owner { get; set; }
        public string proxy { get; set; }
        public List<string> producers { get; set; }
        public Int64 staked { get; set; }
        public String last_vote_weight { get; set; }
        public String proxied_vote_weight { get; set; }
        public bool is_proxy { get; set; }
    }

    /*
     * 
     {"perm_name":"active",
    "parent":"owner",
    "required_auth":
        {"threshold":1,
            "keys":[{"key":"EOS89EDpYUcbrZauAcvAN78EbMzK4kfC7shhuTH4Dr2A3ZYanyCax","weight":1}],
            "accounts":[],
            "waits":[]}}
     * 
     * 
     * */

    public class EOSAccount_permission
    {
        public string parent { get; set; }
        public string perm_name { get; set; }
        public EOSAccount_permission_required_auth required_auth { get; set; }
    }

    public class EOSAccount_permission_required_auth
    {
        //public string accounts { get; set; } = "This Field Not Implemented yet";
        //public string accounts { get; set; } // array of account
        public List<EOSAccount_key> keys { get; set; }
        public int threshold { get; set; }
        //public string waits { get; set; } // array of waits
        //public string waits { get; set; } = "This Field Not Implemented yet";
    }

    public class EOSAccount_key
    {
        public string key { get; set; }
        public int weight { get; set; }

    }


    public class EOSAccount_refundRequest
    {
        public string owner { get; set; }
        public string request_time { get; set; }
        public string net_amount { get; set; }
        public string cpu_amount { get; set; }

        public decimal net_amount_decimal
        {
            get
            {
                string net_amount_clean = string.Empty;
                if (net_amount == null)
                    net_amount_clean = "0.0";
                else
                    net_amount_clean = net_amount.Trim().Replace(" EOS", "");

                return decimal.Parse(net_amount_clean);
            }
        }

        public decimal cpu_amount_decimal
        {
            get
            {
                string cpu_amount_clean = string.Empty;
                if (cpu_amount == null)
                    cpu_amount_clean = "0.0";
                else
                    cpu_amount_clean = cpu_amount.Trim().Replace(" EOS", "");

                return decimal.Parse(cpu_amount_clean);
            }
        }
    }


    /*
                    "total_resources":{"owner":"bob123451234","net_weight":"100000.0000 EOS","cpu_weight":"100000.0000 EOS","ram_bytes":1018841},
                "self_delegated_bandwidth":{"from":"bob123451234","to":"bob123451234","net_weight":"100000.0000 EOS","cpu_weight":"100000.0000 EOS"},
                "refund_request":null,           
                "voter_info":{"owner":"bob123451234","proxy":"","producers":[],"staked":2000000000,"last_vote_weight":"0.00000000000000000","proxied_vote_weight":"0.00000000000000000","is_proxy":0}}
    */
}
