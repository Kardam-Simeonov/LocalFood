﻿using server.Models;

namespace server.Dto
{
    public class OrderAddDto
    {
        public List<ProductOrderDto> Products { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
