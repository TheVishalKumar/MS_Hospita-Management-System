using HospitalManagementSystem.Models.DTO.Appointments;
using HospitalManagementSystem.Models.DTO.Billings;
using HospitalManagementSystem.Services.Appointments;
using HospitalManagementSystem.Services.Billings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystems.API.Controllers.Billings
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    
    public class PrescriptionBillingController : ControllerBase
    {
        private readonly IPrescriptionBillingRepository _prescriptionBilling;
        public PrescriptionBillingController(IPrescriptionBillingRepository prescriptionBilling)
        {
            _prescriptionBilling = prescriptionBilling;
        }

        [HttpPost]
        [Route("CreateBilling")]
        public async Task<object> CreateBilling(CreatePrescriptionBillingDto createPrescriptionBilling)
        {
            return await _prescriptionBilling.CreateBilling(createPrescriptionBilling);
        }

        [HttpGet]
        [Route("GetPrescriptionBillingByAppointmemtId/{billingId}/{appointmemtId}")]
        public async Task<object> GetPrescriptionBillingByAppointmemtId(Guid billingId, Guid appointmemtId)
        {
            return await _prescriptionBilling.GetPrescriptionBillingByAppointmemtId(billingId, appointmemtId);
        }

        [HttpGet]
        [Route("GetBillingByAppointmemtId/{appointmemtId}")]
        public async Task<object> GetBillingByAppointmemtId(Guid appointmemtId)
        {
            return await _prescriptionBilling.GetBillingByAppointmemtId(appointmemtId);
        }

        [HttpGet]
        [Route("GetBillingsByPatientId/{patientId}")]
        public async Task<IActionResult> GetBillingsByPatientId(Guid patientId)
        {
            try
            {
                var data = await _prescriptionBilling.GetBillingsByPatientId(patientId);
                if (data != null && data.Count > 0)
                {
                    return Ok(new { success = true, message = "Billings retrieved successfully", data = data });
                }
                return NotFound(new { success = false, message = "No billings found for this patient", data = (object)null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error retrieving billings: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("GetBillingsByHospitalAndBranch/{hospitalId}/{branchId}")]
        public async Task<IActionResult> GetBillingsByHospitalAndBranch(Guid hospitalId, Guid branchId)
        {
            try
            {
                var data = await _prescriptionBilling.GetBillingsByHospitalAndBranch(hospitalId, branchId);
                if (data != null && data.Count > 0)
                {
                    return Ok(new { success = true, message = "Billings retrieved successfully", data = data });
                }
                return NotFound(new { success = false, message = "No billings found for this hospital and branch", data = (object)null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error retrieving billings: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("GetPendingBillings/{hospitalId}/{branchId}")]
        public async Task<IActionResult> GetPendingBillings(Guid hospitalId, Guid branchId)
        {
            try
            {
                var data = await _prescriptionBilling.GetPendingBillings(hospitalId, branchId);
                if (data != null && data.Count > 0)
                {
                    return Ok(new { success = true, message = "Pending billings retrieved successfully", data = data });
                }
                return NotFound(new { success = false, message = "No pending billings found", data = (object)null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error retrieving pending billings: {ex.Message}" });
            }
        }
    }
}
