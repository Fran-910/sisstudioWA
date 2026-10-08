using sisstudioWA.BD.Datos;
using sisstudioWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Repositorio.Repositorios
{
    public class CarritoRepositorio : Repositorio<Carrito>, ICarritoRepositorio
    {
        private readonly AppDbContext context;

        public CarritoRepositorio(AppDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
