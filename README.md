# EOSDotNet - EOS for enterprise

  

EOSDotNet is cross plaform .NET core library intended to make interacting with the EOS blockchain (https://eos.io/) a pleasure.

  

As much as possible the library tries to provide strongly typed classes which mirror the EOS structure. This helps to prevent  runtime exceptions and provides excellent code completion and improved developer usability.

  

The library is still under development, but currently provides a well structured framework for interacting with Json RPC API and system tables like the producers, voters, namebids, etc.

  

Complexity such as pagination when dealing with tables is currently handled for the user.

  

Plans are to expand the library to support all functions that are currently available through the cleos command line tool.

  

If you're a .NET developer and would like to contribute to the project, we'd welcome all contributions.

  
  

## What's in the box?

  

This repository currently contains three projects:

  

*  **EOSNewYork.EOSCore** - This the library which can be included into your own project to build great things.

  

*  **cscleos** - This is an example project which makes use of the EOSDotNet class library. The commandline tool allows users to query system tables on the EOS Blockchain easily. As functionality is added to the EOSDotNet class library, this tool will expanded with useful features that leverage those functions.

  

*  **EOSLibConsole** - A scratch pad with some test which don't yet fit into the cscleos tool.

### EOSNewYork.EOSCore
All functionality in this library is divided into 4 APIs - 
 - ChainAPI (accessed via EOSNewYork.EOSDotNet.ChainAPI class) 
 - HistoryAPI (accessed via EOSNewYork.EOSDotNet.HistoryAPI class)
 - TableAPI (accessed via EOSNewYork.EOSDotNet.TableAPI class)
 - KeyManager (accessed via EOSNewYork.EOSDotNet.Utilities.KeyManager class)

  Each API class has various helper functions that can be called synchronously or asynchronously. For example to convert ABI json to binary format you can call -
```
var chainAPI = new ChainAPI("https://api.eosnewyork.io");
      
string  _code  =  "eosio.token", _action  =  "transfer", _memo  =  "";
TransferArgs  _args  =  new  TransferArgs(){ from  =  "account1", to  =  "account2", quantity  =  "1.0000 EOS", memo  =  _memo };
      
//called asynchronously
var abiJsonToBinAsync = await chainAPI.GetAbiJsonToBinAsync(_code, _action, _args);
//called synchronously
var abiJsonToBinSync = chainAPI.GetAbiJsonToBin(_code, _action, _args);
```
  
 

#### ChainAPI
##### PushTransaction
Build, sign and push a transaction
###### Method Signatures
```
public async Task<PushTransaction> PushTransactionAsync(Action[] actions, List<string> privateKeysInWIF);
public PushTransaction PushTransaction(Action[] actions, List<string> privateKeysInWIF);
```
###### Example
```
string _accountName1 = "account1", _accountName2 = "account2", _permissionName = "active", _code = "eosio.token", _action = "transfer", _memo = "", privateKeyWIF = "...";

//prepare arguments to be passed to action
TransferArgs _args = new TransferArgs(){ from = _accountName1, to = _accountName2, quantity = "1.0000 EOS", memo = _memo };

//prepare action object
Action action = new ActionUtility(host).GetActionObject(_accountName, _action, _permissionName, _code, _args);

//get private keys to be used to sign
List<string> privateKeysInWIF = new List<string> { privateKeyWIF };

//delaySec parameter defaults to 30, to set it to a different value create chainAPI object as
//var chainAPI = new ChainAPI("https://api.eosnewyork.io", 120)

//called asynchronously
var transactionResultAsync = await chainAPI.PushTransactionAsync(new [] { action }, privateKeysInWIF);
//called synchronously
var transactionResultSync = chainAPI.PushTransaction(new [] { action }, privateKeysInWIF);
```
&nbsp; 
##### GetAbiBinToJson
Converts binary abi to json
###### Method Signatures
```
public  async  Task<AbiBinToJson> GetAbiBinToJsonAsync(string  code, string  action, string  binargs);
public  AbiBinToJson  GetAbiBinToJson(string  code, string  action, string  binargs);
```
###### Example
```
string  _code  =  "eosio.token", _action  =  "transfer", _binargs  =  "...";

//called asynchronously
var  abiBinToJsonAsync  =  await chainAPI.GetAbiBinToJsonAsync(_code, _action, _binargs);
//called synchronously
var  abiBinToJsonSync  =  chainAPI.GetAbiBinToJson(_code, _action, _binargs);
```
&nbsp; 
##### GetAbiJsonToBin
Converts json abi to binary
###### Method Signatures
```
public async Task<AbiJsonToBin> GetAbiJsonToBinAsync(string code, string action, object args);
public AbiJsonToBin GetAbiJsonToBin(string code, string action, object args);
```
###### Example
```
string  _code  =  "eosio.token", _action  =  "transfer", _memo  =  "";
TransferArgs  _args  =  new  TransferArgs(){ from  =  "account1", to  =  "account2", quantity  =  "1.0000 EOS", memo  =  _memo };
  
//called asynchronously
var abiJsonToBinAsync = await chainAPI.GetAbiJsonToBinAsync(_code, _action, _args);
//called synchronously
var abiJsonToBinSync = chainAPI.GetAbiJsonToBin(_code, _action, _args);
```
&nbsp; 
##### GetAccount
Get account details
###### Method Signatures
```
public async Task<Account> GetAccountAsync(string accountName);
public Account GetAccount(string accountName);
```
###### Example
```
//called asynchronously
var accountAsync = await chainAPI.GetAccountAsync("account1");
//called synchronously
var accountSync = chainAPI.GetAccount("account1");
```
&nbsp; 
##### GetBlock
Get block details
###### Method Signatures
```
public async Task<Block> GetBlockAsync(string blockNumOrId);
public Block GetBlock(string blockNumOrId);
```
###### Example
```
//called asynchronously
var blockAsync = await chainAPI.GetBlockAsync(100);
//called synchronously
var blockSync = chainAPI.GetBlock(100);
```
&nbsp; 
##### GetAbi
Get abi for contract
###### Method Signatures
```
public async Task<Abi> GetAbiAsync(string accountName);
public Abi GetAbi(string accountName);
```
###### Example
```
//called asynchronously
var abiAsync = await chainAPI.GetAbiAsync("eosio");
//called synchronously
var abiSync = chainAPI.GetAbi("eosio");
```
&nbsp; 
##### GetCode
Get code for contract
###### Method Signatures
```
public async Task<Code> GetCodeAsync(string accountName, bool codeAsWasm);
public Code GetCode(string accountName, bool codeAsWasm);
```
###### Example
```
//called asynchronously
var codeAsync = await chainAPI.GetCodeAsync("eosio", true);
//called synchronously
var codeSync = chainAPI.GetCode("eosio", true);
```
&nbsp; 
##### GetRawCodeAndAbi
Get raw code and abi for contract
###### Method Signatures
```
public async Task<RawCodeAndAbi> GetRawCodeAndAbiAsync(string accountName);
public RawCodeAndAbi GetRawCodeAndAbi(string accountName);
```
###### Example
```
//called asynchronously
var rawCodeAndAbiAsync = await chainAPI.GetRawCodeAndAbiAsync("eosio");
//called synchronously
var rawCodeAndAbiSync = chainAPI.GetRawCodeAndAbi("eosio");
```
&nbsp; 
##### GetCurrencyBalance
Get currency balance
###### Method Signatures
```
public async Task<CurrencyBalance> GetCurrencyBalanceAsync(string account, string code, string symbol);
public CurrencyBalance GetCurrencyBalance(string account, string code, string symbol);
```
###### Example
```
//called asynchronously
var currencyBalanceAsync = await chainAPI.GetCurrencyBalanceAsync("account1", "eosio.token", "EOS");
//called synchronously
var currencyBalanceSync = chainAPI.GetCurrencyBalance("account1", "eosio.token", "EOS");
```
&nbsp; 
##### GetInfo
Get blockchain info
###### Method Signatures
```
public async Task<Info> GetInfoAsync();
public Info GetInfo();
```
###### Example
```
//called asynchronously
var infoAsync = await chainAPI.GetInfoAsync();
//called synchronously
var infoSync = chainAPI.GetInfo();
```
&nbsp; 
##### GetProducerSchedule
Get producer schedules
###### Method Signatures
```
public async Task<ProducerSchedule> GetProducerScheduleAsync();
public ProducerSchedule GetProducerSchedule();
```
###### Example
```
//called asynchronously
var producerScheduleAsync = await chainAPI.GetProducerScheduleAsync();
//called synchronously
var producerScheduleSync = chainAPI.GetProducerSchedule();
```
&nbsp; 
#### HistoryAPI
##### GetActions
Get actions for account
###### Method Signatures
```
public async Task<Actions> GetActionsAsync(int pos, int offset, string accountName);
public Actions GetActions(int pos, int offset, string accountName);
```
###### Example
```
//called asynchronously
var actionsAsync = await historyAPI.GetActionsAsync(-1, 100, "eosio");
//called synchronously
var actionsSync = historyAPI.GetActions(-1, 100, "eosio");
```
&nbsp; 
##### GetTransaction
Get a transaction
###### Method Signatures
```
public async Task<TransactionResult> GetTransactionAsync(string id, uint? blockNumHint);
public TransactionResult GetTransaction(string id, uint? blockNumHint);
```
###### Example
```
string  id  =  "ebe3435b22e302c6e3021b97756cdd900099eeac9060db3dbd1b116c7bbeee69";

//called asynchronously
var transactionAsync = await historyAPI.GetTransactionAsync(id, 11371727);  
//called synchronously
var transactionSync = historyAPI.GetTransaction(id, 11371727);  
```
&nbsp; 
#### TableAPI
##### GetGlobalRows
Get global rows
###### Method Signatures
```
public async Task<List<GlobalRow>> GetGlobalRowsAsync();
public List<GlobalRow> GetGlobalRows();
```
###### Example
```
//called asynchronously
var globalRowsAsync = await tableAPI.GetGlobalRowsAsync();
//called synchronously
var globalRowsSync = tableAPI.GetGlobalRows();
```
&nbsp; 
##### GetNameBidRows
Get name bid rows
###### Method Signatures
```
public async Task<List<NameBidsRow>> GetNameBidRowsAsync();
public List<NameBidsRow> GetNameBidRows();
```
###### Example
```
//called asynchronously
var nameBidRowsAsync = await tableAPI.GetNameBidRowsAsync();
//called synchronously
var nameBidRowsSync = tableAPI.GetNameBidRows();
```
&nbsp; 
##### GetProducerRows
Get producer rows
###### Method Signatures
```
public async Task<List<ProducerRow>> GetProducerRowsAsync();
public List<ProducerRow> GetProducerRows();
```
###### Example
```
//called asynchronously
var producerRowsAsync = await tableAPI.GetProducerRowsAsync();
//called synchronously
var producerRowsSync = tableAPI.GetProducerRows();
```
&nbsp; 
##### GetVoterRows
Get voter rows
###### Method Signatures
```
public async Task<List<VoterRow>> GetVoterRowsAsync();
public List<VoterRow> GetVoterRows();
```
###### Example
```
//called asynchronously
var voterRowsAsync = await tableAPI.GetVoterRowsAsync();
//called synchronously
var voterRowsSync = tableAPI.GetVoterRows();
```
&nbsp; 
#### KeyManager
##### GenerateKeyPair
Generate new keypair
###### Method Signature
```
public static KeyPair GenerateKeyPair();
```
###### Example
```
var keypair = KeyManager.GenerateKeyPair();
```

### cscleos

  

Simply running cscleos will output help that looks something like this:

  

```

Usage - cscleos <action> -options

  

GlobalOption Description

Help (-?) Shows this help

  

Actions

  

getKnownTable <table> <outputFormat> [<fieldList>] [<delimiter>] - Retrieve data from one of the well known EOS tables

  

Option Description

table* (-t) The name of the table

producers

voters

global

namebids

outputFormat* (-o) The format of the result. Defaults to Tab Seperated [Default='csv']

csv

json

delimiter (-d) The characted to use as a delimiter when outputting as a CSV. Default is a tab. [Default=' ']

fieldList (-f) A comma separated list of fields that should be returned. Default is to return all fields.

```

  

Example usage:

  

```

#By default the output is in TSV format.

cscleos.exe getKnownTable namebids

  

#Specify -o json to output in json format

cscleos.exe getKnownTable namebids -o json

  

#Specify -d , to get the output in a csv format (comma is used as the delimiter as apposed to the default tab)

cscleos.exe getKnownTable namebids -d ,

  

#If you're only interested in specific fields, specify the -f and then provide a comma separated list of fields that you would like included.

cscleos.exe getKnownTable namebids -f newname,high_bid

  

```

  

Fetch all producers

  

```

# An example fetching all producers into a CSV

# cscleos.exe getKnownTable producers -d , | more

  

owner,total_votes,producer_key,is_active,unpaid_blocks,url,total_votes_long

123singapore,9680982131305682.00000000000000000,EOS71UbkZzuz55WNBpsEVQzkXrZAJ2XyLoQiEcS9WKwbYambhFxWb,True,0,http://eos.vote,9.68098213130568E+15

1eosbattles1,329144938830.10693359375000000,EOS1111111111111111111111111111111114T1Anm,False,0,http://eosbattles.com,329144938830.107

1eostheworld,7127308523679749.00000000000000000,EOS7DtVzfmr1c5ACb7usAwyn4f39V7urk6kBWswUWCtg3H8H6CFAr,True,0,https://eostheworld.io,7.12730852367975E+15

1teamcanada1,97028173128.15544128417968750,EOS7gBHpJMaWo8uzcXRfBAhN9yLfVTLTbH4L2xA64B5HNMnZpVuKJ,True,0,http://can-play.ca,97028173128.1554

acroeos12345,26187829550021016.00000000000000000,EOS56PyKoHXwyRkwDzr2uNhgDRioPoq5TwdN8Pm2hQGko7jrup2W5,True,0,http://www.acroeos.one,2.6187829550021E+16

activeeoscom,434395878933955.00000000000000000,EOS788SrVzZ3mJt3WUZkmYadjFJCisJGhZRTp85EznxEaqsARN26w,True,0,https://www.activeeos.com,434395878933955

alohaeosprod,5816532095666617.00000000000000000,EOS53pfXfxZ4tH3EGccdnGvBZVJsrcSf2nbCKiLLMphgaii9XxxhM,True,0,https://www.alohaeos.com,5.81653209566662E+15

argentinaeos,169687487348078496.00000000000000000,EOS7n4UUEDQRWeJ5UmCf9yqWXY5fsTtbo78HyYa5uBbM1xwa5DwRj,True,5522,https://www.eosargentina.io,1.69687487348078E+17

atticlabeosb,42191177849587144.00000000000000000,EOS7LqudX6Ac4woGwTF9CvQKwrJhg3H7p3pScDoXj1Ws82mMZFQqf,True,0,http://atticlab.net/,4.21911778495871E+16

atticlabeosp,13342565292388.24609375000000000,EOS1111111111111111111111111111111114T1Anm,False,0,http://atticlab.net/,13342565292388.2

aus1genereos,86798290100405904.00000000000000000,EOS57ZyzVjoEG2bvzmYmmeiH8YnYtRudxNKxppx17q7Hg29F8tW4t,True,0,http://www.genereos.io,8.67982901004059E+16

bchainlabeos,1054617659343577.37500000000000000,EOS6n6RZi6EXHKBX53HyuXgzMw5sUzWWQomwDLYLc1PcjZY3CCzzE,True,0,https://blockchainlab.me,1.05461765934358E+15

bitcoineosfu,13385780843681.28320312500000000,EOS5KvXdnwJV4YGcv7XL3K4UKsDSZxA13gmubYLTvE14g9vrMf7iT,True,0,http://bitcoineos.fun,13385780843681.3

bitcoingod11,26871852551978.96484375000000000,EOS8FGBXiineqciyhjKM2ypFgETDu8BJTDyHXuA2kY58Rbt1RNeHm,True,0,https://eos.bitcoingod.org/,26871852551979

bitfinexeos1,191486379979670112.00000000000000000,EOS6sgKjHUFtY1XxxQaMDwfxBac6nDBibVzZHb8LFMVmvSjcCdDhE,True,6247,https://www.bitfinex.com,1.9148637997967E+17

bitspacenode,16823859142716942.00000000000000000,EOS7HvfCTxaL4DwokbZRsbrXXQafzE6wcG38azf6WefKGHBqsE3Ad,True,0,https://eos.bitspace.no,1.68238591427169E+16

```

  


## Installation

  
  
  

1. Install .NET core on your OS (Yes, this really works on Linux, and macOS) - easy to follow instructions can be found here

https://www.microsoft.com/net/learn/get-started

  

2. Clone the repo

```

git clone https://github.com/eosnewyork/EOSDotNet.git

cd EOSDotNet

```

  

Execute one of the following commands to create binaries:

  

```

dotnet publish -c release -r win10-x64

dotnet publish -c release -r osx.10.10-x64

dotnet publish -c release -r ubuntu.14.04-x64

```

Output will differ based on your system. Here's some example output from Ubuntu

  

```

$dotnet publish -c release -r ubuntu.14.04-x64

Microsoft (R) Build Engine version 15.7.179.6572 for .NET Core

Copyright (C) Microsoft Corporation. All rights reserved.

  

Restoring packages for /home/ubuntu/test1/EOSDotNet/EOSDotNet/EOSDotNet.csproj...

Generating MSBuild file /home/ubuntu/test1/EOSDotNet/EOSDotNet/obj/EOSDotNet.csproj.nuget.g.props.

Generating MSBuild file /home/ubuntu/test1/EOSDotNet/EOSDotNet/obj/EOSDotNet.csproj.nuget.g.targets.

Restore completed in 253.11 ms for /home/ubuntu/test1/EOSDotNet/EOSDotNet/EOSDotNet.csproj.

Restoring packages for /home/ubuntu/test1/EOSDotNet/cscleos/cscleos.csproj...

Generating MSBuild file /home/ubuntu/test1/EOSDotNet/cscleos/obj/cscleos.csproj.nuget.g.props.

Generating MSBuild file /home/ubuntu/test1/EOSDotNet/cscleos/obj/cscleos.csproj.nuget.g.targets.

Restore completed in 51.47 ms for /home/ubuntu/test1/EOSDotNet/cscleos/cscleos.csproj.

Restoring packages for /home/ubuntu/test1/EOSDotNet/EOSLibConsole/EOSLibConsole.csproj...

Generating MSBuild file /home/ubuntu/test1/EOSDotNet/EOSLibConsole/obj/EOSLibConsole.csproj.nuget.g.props.

Generating MSBuild file /home/ubuntu/test1/EOSDotNet/EOSLibConsole/obj/EOSLibConsole.csproj.nuget.g.targets.

Restore completed in 23.63 ms for /home/ubuntu/test1/EOSDotNet/EOSLibConsole/EOSLibConsole.csproj.

EOSDotNet -> /home/ubuntu/test1/EOSDotNet/EOSDotNet/bin/Release/netcoreapp2.1/EOSDotNet.dll

EOSDotNet -> /home/ubuntu/test1/EOSDotNet/EOSDotNet/bin/Release/netcoreapp2.1/ubuntu.14.04-x64/EOSDotNet.dll

EOSDotNet -> /home/ubuntu/test1/EOSDotNet/EOSDotNet/bin/Release/netcoreapp2.1/ubuntu.14.04-x64/publish/

EOSLibConsole -> /home/ubuntu/test1/EOSDotNet/EOSLibConsole/bin/Release/netcoreapp2.1/ubuntu.14.04-x64/EOSLibConsole.dll

EOSLibConsole -> /home/ubuntu/test1/EOSDotNet/EOSLibConsole/bin/Release/netcoreapp2.1/ubuntu.14.04-x64/publish/

cscleos -> /home/ubuntu/test1/EOSDotNet/cscleos/bin/Release/netcoreapp2.1/ubuntu.14.04-x64/cscleos.dll

cscleos -> /home/ubuntu/test1/EOSDotNet/cscleos/bin/Release/netcoreapp2.1/ubuntu.14.04-x64/publish/

  

```

  

```

# Change directly to the cscleos publish folder

cd cscleos/bin/Release/netcoreapp2.1/ubuntu.14.04-x64/publish/

#Run the cscleos without params to print the help

./cscleos

```