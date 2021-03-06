namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data;
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class LoansController : ControllerBase
    {
        private readonly ILoanRepository loanRepository;
        private readonly DataContext dataContext;

        private readonly IInternRepository internRepository;
        private readonly IApplicantRepository applicantRepository;
        public LoansController(ILoanRepository loanRepository, DataContext dataContext, IInternRepository internRepository, IApplicantRepository applicantRepository)
        {
            this.loanRepository = loanRepository;
            this.dataContext = dataContext;
            this.internRepository = internRepository;
            this.applicantRepository = applicantRepository;
        }

        //[HttpPost]
        //[Route("GetLoanByEmail")]
        public IActionResult GetLoansController()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(this.loanRepository.GetLoans());
            //return Ok(this.loanRepository.GetLoansWithInternsAndLoanDetails());
            //return Ok(this.loanRepository.GetLoansWithLoanDetailsAndMaterial());
            //return Ok(this.loanRepository.GetLoanWithLoanDetailsById(1));
            //return Ok(this.loanRepository.GetLoanWithLoanDetailsAndMaterialById(1));
            //return Ok(this.loanRepository.GetLoansWithLoanDetailsWithMaterialAndOwnerByNameMaterial("VGA"));

        }
        
        [HttpPost]
        public async Task<IActionResult> PostLoan([FromBody] MAV.Common.Models.LoanRequest loan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var intern = this.internRepository.GetInternByName(loan.Intern);
            if (intern == null)
            {
                return BadRequest("intern not found");
            }
            var applicant = this.applicantRepository.GetApplicantByName(loan.Applicant);
            if (applicant == null)
            {
                return BadRequest("applicant not found");
            }
            var entityLoan = new MAV.Web.Data.Entities.Loan
            {
                Intern = intern,
                Applicant = applicant
            };
            
            if (entityLoan == null)
            {
                return BadRequest("loan not found");
            }
            var newLoan = await this.loanRepository.CreateAsync(entityLoan);
            return Ok(newLoan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan([FromRoute] int id, [FromBody] LoanRequest loan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != loan.Id)
            {
                return BadRequest();
            }

            var oldLoan = await this.loanRepository.GetByIdAsync(id);
            if (oldLoan == null)
            {
                return BadRequest("loan not found");
            }

            var intern = this.internRepository.GetInternByName(loan.Intern);
            if (intern == null)
            {
                return BadRequest("intern not found");
            }
            var applicant = this.applicantRepository.GetApplicantByName(loan.Applicant);
            if (applicant == null)
            {
                return BadRequest("applicant not found");
            }

            oldLoan.Intern = intern;
            oldLoan.Applicant = applicant;

            var updateLoan = await this.loanRepository.UpdateAsync(oldLoan);

            return Ok(updateLoan);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldLoan = await this.loanRepository.GetByIdAsync(id);

            if (oldLoan == null)
            {
                return BadRequest("Id not found");
            }
            ;
            await this.loanRepository.DeleteAsync(oldLoan);
            return Ok(oldLoan);

        }
    }
}