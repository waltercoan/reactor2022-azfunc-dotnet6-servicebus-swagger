using System;
using System.Collections.Generic;
using Azure;
using Microsoft.WindowsAzure.Storage;
using Azure.Data.Tables;

namespace azfunc.Model;
public class VoteRegistry
{
    public string MusicName { get; set; }
    public int Quantity { get; set; }
}