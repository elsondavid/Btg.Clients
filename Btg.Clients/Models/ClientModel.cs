﻿namespace Btg.Clients.Models;

public class ClientModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public int? Age { get; set; }
    public string Address { get; set; }
}