using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId {get;set;}
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime Date {get;set;}
        public string Address {get;set;}
        public int UserId {get;set;}
        public ActivityUser Planner {get;set;}
        public List<Response> Responses {get;set;}
    }
    public class Response
    {
        [Key]
        public int ResponseId {get;set;}
        public int ActivityId {get;set;}
        public int UserId {get;set;}
        public ActivityUser Guest {get;set;}
    }
    

}