using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monkey.Sql.Scripts;

namespace Monkey.Sql.WebApiHost.Controllers
{
    public class DatabaseController : ControllerBase
    {
        private readonly IScriptManager _scriptManager;

        public DatabaseController(IScriptManager scriptManager)
        {
            _scriptManager = scriptManager;
        }

        [HttpPost]
        [Route("api/Install")]
        public async Task Install(string connectionStringName)
        {
            await _scriptManager.InstallExternal(connectionStringName);
        }
    }
}
