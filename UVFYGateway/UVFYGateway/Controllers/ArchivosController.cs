using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UVFYGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        private GrpcChannel ServicioDeArchivos;
        private readonly ILogger<ArchivosController> _logger;
        public ArchivosController (ILogger<ArchivosController> logger)
        {
            _logger = logger;
            GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
            grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            ServicioDeArchivos = GrpcChannel.ForAddress("http://172.17.0.7:80", grpcChannelOptions);
        }


    }
}