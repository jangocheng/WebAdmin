using System;
using System.Collections.Generic;
using System.Text;

namespace MSDev.Task.Entities
{
    class MvaDetailInfoEntity
    {
    }
    public class Metadata
    {
        public string title { get; set; }
    }

    public class Metadata2
    {
        public string description { get; set; }
    }

    public class Metadata3
    {
        public string description { get; set; }
        public string duration { get; set; }
        public string learningresourcetype { get; set; }
        public int points { get; set; }
    }

    public class Resource
    {
        public string __invalid_name__href { get; set; }
        public string __invalid_name__scormtype { get; set; }
        public Metadata3 metadata { get; set; }
    }

    public class Item2
    {
        public string __invalid_name__identifier { get; set; }
        public string __invalid_name__isdeleted { get; set; }
        public string __invalid_name__isvisible { get; set; }
        public Metadata2 metadata { get; set; }
        public Resource resource { get; set; }
        public string title { get; set; }
    }

    public class Metadata4
    {
        public string description { get; set; }
        public string createdVersion { get; set; }
        public string version { get; set; }
    }

    public class Objective
    {
        public int __invalid_name__objectiveID { get; set; }
        public string objective { get; set; }
    }

    public class Metadata5
    {
        public string description { get; set; }
        public string duration { get; set; }
        public string learningresourcetype { get; set; }
        public int points { get; set; }
    }

    public class Resource2
    {
        public string __invalid_name__href { get; set; }
        public string __invalid_name__scormtype { get; set; }
        public Metadata5 metadata { get; set; }
    }

    public class Item
    {
        public string __invalid_name__identifier { get; set; }
        public string __invalid_name__isdeleted { get; set; }
        public string __invalid_name__isvisible { get; set; }
        public List<Item2> item { get; set; }
        public Metadata4 metadata { get; set; }
        public List<Objective> Objectives { get; set; }
        public string title { get; set; }
        public Resource2 resource { get; set; }
    }

    public class Organization
    {
        public string __invalid_name__identifier { get; set; }
        public List<Item> item { get; set; }
        public string title { get; set; }
    }

    public class Organizations
    {
        public string __invalid_name__default { get; set; }
        public List<Organization> organization { get; set; }
    }

    public class Manifest
    {
        public Metadata metadata { get; set; }
        public Organizations organizations { get; set; }
    }

    public class RootObject
    {
        public Manifest manifest { get; set; }
    }
}
