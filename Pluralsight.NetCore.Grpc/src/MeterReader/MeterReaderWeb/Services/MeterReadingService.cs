using Grpc.Core;
using MeterReader.gRPC;
using MeterReaderWeb.Data;
using MeterReaderWeb.Data.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static MeterReader.gRPC.MeterReaderService;

namespace MeterReaderWeb.Services
{
    public class MeterReadingService : MeterReaderServiceBase
    {
        private readonly IReadingRepository _repository;
        private readonly ILogger<MeterReadingService> _logger;

        public MeterReadingService(IReadingRepository repository, I`Logger<MeterReadingService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<StatusMessage> AddReading(ReadingPacket request, ServerCallContext context)
        {
            if (request.Successful == ReadingStatus.Success)
            {
                foreach (var reading in request.Redings)
                {
                    var readingValue = new MeterReading()
                    {
                        CustomerId = reading.CustomerId,
                        Value = reading.ReadingValue,
                        ReadingDate = reading.ReadingTime.ToDateTime(),
                    };

                    _repository.AddEntity(readingValue);
                }
            }

            if (await _repository.SaveAllAsync())
            {
                return new StatusMessage()
                {
                    //Notes = "tesss",
                    Success= ReadingStatus.Success
                };
            }

             return new StatusMessage()
                {
                    //Notes = "Failed",
                    Success= ReadingStatus.Failure
                };
        }
    }
}
