using AMTestApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMTestApplication.Services.Implementations
{
    public class BusinessLogic : IBusinessLogic
    {
        private ILoggerService loggerService;
        private IDatabaseService databaseService;
        public BusinessLogic(ILoggerService loggerService, IDatabaseService databaseService)
        {
            this.loggerService = loggerService;
            this.databaseService = databaseService;
        }

        public async Task<DataTable> CalculateDataAsync()
        {
            loggerService.Info("Processing data");

            await databaseService.ExecuteNonQueryAsync(
@"
INSERT INTO [Data]
(
 [cell]
,[value]
)
select 
	c11 + '_' + c21 as [cell],
	cast(POWER(cast(2 as float),cast ((c12-c22) as float)) as decimal(18,2)) as [value]
from
(
	select * from (Select top 3 [Column1] as c11, [Column6] as c12 from [RawData] WHERE Column1 IN ('C01','D01','F01')) t1
	cross join (Select top 3 [Column1] as c21, [Column6] as c22 from [RawData] WHERE Column1 IN ('E01','G01','H01')) t2
) t"
                );

            return await databaseService.DataGetAsync();
        }

    }
}
