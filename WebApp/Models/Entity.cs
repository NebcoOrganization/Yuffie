using System;
using System.Collections.Generic;
using System.Linq;

namespace Yuffie.WebApp.Models {

    public class Entity {
      public int Id {get;set;}
        public DateTime Date {get;set;}
        public string Value {get;set;}
    }

    public class entity
    {
        public long Id {get;set;}
        public DateTime Date {get;set;}
        public Dictionary<string, object> Data {get;set;}
        public entity Entity  {get;set;}
    }
}