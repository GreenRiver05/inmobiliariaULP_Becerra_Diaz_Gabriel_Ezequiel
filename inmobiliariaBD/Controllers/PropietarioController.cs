using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliariaBD.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace inmobiliariaBD.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly IConfiguration config;
        private readonly IRepositorioPropietario repositorio;

        public PropietarioController(IConfiguration config, IRepositorioPropietario repo)
        {
            this.config = config;
            this.repositorio = repo;
        }

    }



}