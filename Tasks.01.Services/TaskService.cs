using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Tasks._01.Repository;

namespace Tasks._01.Services
{
    public class TaskService
    {
        ContaClienteRepository r_Repositorio = null;
        ContaClienteService r_Servico = null;

        public TaskService()
        {
            r_Repositorio = new ContaClienteRepository();
            r_Servico = new ContaClienteService();
        }

        public void Do()
        {
            
        }
    }
}
