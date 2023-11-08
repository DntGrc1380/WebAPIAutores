﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAutores.Filtros
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> logger;

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger) {
            this.logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("ANTES de ejecutar acción");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("DESPUÉS de ejecutar acción");
        }

        
    }
}
