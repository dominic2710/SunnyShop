using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Controllers.ClassifyName;
using SellManagement.Api.Functions;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ClassifyNameController : Controller
    {
        IClassifyNameFunction _classifyNameFunction;
        public ClassifyNameController(IClassifyNameFunction classifyNameFunction)
        {
            _classifyNameFunction = classifyNameFunction;
        }

        [HttpPost("GetNameByGroupIdAndCode")]
        [Authorize]
        public async Task<IActionResult> GetNameByGroupIdAndCode([FromBody] GetNameByGroupIdAndCodeRequest request)
        {
            var response = new GetNameByGroupIdAndCodeResponse
            {
                ClassifyNameData = await _classifyNameFunction.GetNameByGroupIdAndCode(request.GroupId, request.Code)
            };
            return Ok(response);
        }

        [HttpPost("GetListClassifyNameByGroupId")]
        [Authorize]
        public async Task<IActionResult> GetListClassifyNameByGroupId([FromBody] int groupId)
        {
            var response = new GetListClassifyNameByGroupIdResponse
            {
                ListClassifyName = await _classifyNameFunction.GetListNameByGroupId(groupId)
            };
            return Ok(response);
        }

        [HttpPost("UpdateClassifyName")]
        [Authorize]
        public async Task<IActionResult> UpdateClassifyName([FromBody] ClassifyNameUpdateRequest request)
        {
            await _classifyNameFunction.DeleteAllName(request.ClassifyNameData.ElementAt(0).GroupId);

            foreach (var classifyName in request.ClassifyNameData)
            {
                await _classifyNameFunction.AddName(classifyName);
            }
            var response = new ClassifyNameUpdateResponse
            {
                UpdRecCount = 1
            };
            return Ok(response);
        }
    }
}
