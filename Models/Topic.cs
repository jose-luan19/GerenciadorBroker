﻿namespace Models
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; }
        public string RoutingKey { get; set; }
    }
}