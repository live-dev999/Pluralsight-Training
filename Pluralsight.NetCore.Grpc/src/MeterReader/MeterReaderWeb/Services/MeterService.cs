using Grpc.Core;
using MeterReader.gRPC;
using MeterReaderWeb.Data;
using MeterReaderWeb.Data.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static MeterReader.gRPC.MeterReaderService;

namespace MeterReaderWeb.Services
{
    public class MeterService : MeterReaderServiceBase
    {
        private readonly IReadingRepository _repository;
        private readonly ILogger<MeterService> _logger;

        public MeterService(IReadingRepository repository, ILogger<MeterService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<StatusMessage> AddReading(ReadingPacket request, ServerCallContext context)
        {
            var result = new StatusMessage()
            {
                Success = ReadingStatus.Failure
            };

            if (request.Successful == ReadingStatus.Success)
            {
                try
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
                    if (await _repository.SaveAllAsync())
                        result.Success = ReadingStatus.Success;
                }
                catch (System.Exception ex)
                {

                    result.Message = "Exception throw during process";
                    _logger.LogError($"Exception throw during saveing of readings : {ex}");
                }
            }
            return result;
        }
    }
}
