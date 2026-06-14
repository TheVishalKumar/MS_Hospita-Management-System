using AutoMapper;
using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Models.DTO.Medicines;
using HospitalManagementSystem.Models.Models.Medicines;
using HospitalManagementSystem.Models.DTO.Patients;
using HospitalManagementSystem.Models.Models.Patients;
using HospitalManagementSystem.Models.DTO.Diseases;
using HospitalManagementSystem.Models.Models.Diseases;
using HospitalManagementSystem.Models.Models.Categories;
using HospitalManagementSystem.Models.Models.Users;
using HospitalManagementSystem.Models.DTO.Wards;
using HospitalManagementSystem.Models.Models.Wards;
using HospitalManagementSystem.Models.DTO.Rooms;
using HospitalManagementSystem.Models.Models.Rooms;
using HospitalManagementSystem.Models.DTO.Hospitals;
using HospitalManagementSystem.Models.Models.Hospitals;
using HospitalManagementSystem.Models.DTO.HospitalBranches;
using HospitalManagementSystem.Models.Models.HospitalBranches;
using HospitalManagementSystem.Models.DTO.Doctors;
using HospitalManagementSystem.Models.Models.Doctors;
using HospitalManagementSystem.Models.DTO.Appointments;
using HospitalManagementSystem.Models.Models.Appointments;
using HospitalManagementSystem.Models.DTO.Prescriptions;
using HospitalManagementSystem.Models.Models.Prescriptions;
using HospitalManagementSystem.Models.DTO.Billings;
using HospitalManagementSystem.Models.Models.Billings;
using HospitalManagementSystem.Models.Models.Dashboards;
using HospitalManagementSystem.Models.DTO.Dashboards;

namespace HospitalManagementSystem.API.AutoMapper
{
    public class EntityAutoMapper: Profile
    {
        public EntityAutoMapper()
        {
            CreateMap<CreateUpdateCategoryDto, Category>();
            CreateMap<Category, GetCategoryDto>();
            CreateMap<Category, List<GetCategoryDto>>();

            CreateMap<CreateUserDetailsDto, UserDetails>();
            CreateMap<UpdateUserDetailsDto, UserDetails>();
            CreateMap<UserDetails, GetUsersDetailsDto>();
            CreateMap<UserDetails, List<GetUsersDetailsDto>>();

            CreateMap<CreateMedicineDto, MedicineMaster>();
            CreateMap<UpdateMedicineDto, MedicineMaster>();
            CreateMap<MedicineMaster, GetMedicinesDto>();
            CreateMap<MedicineMaster, List<GetMedicinesDto>>();

            CreateMap<CreatePatientDto, PatientDetails>();
            CreateMap<UpdatePatientDto, PatientDetails>();
            CreateMap<PatientDetails, GetPatientDto>();
            CreateMap<PatientDetails, List<GetPatientDto>>();

            CreateMap<CreateDiseaseDto, DiseaseMaster>();
            CreateMap<UpdateDiseaseDto, DiseaseMaster>();
            CreateMap<DiseaseMaster, GetDiseaseListDto>();
            CreateMap<DiseaseMaster, List<GetDiseaseListDto>>();

            CreateMap<CreateWardDto, WardMaster>();
            CreateMap<UpdateWardDto, WardMaster>();
            CreateMap<WardMaster, GetWardDto>();
            CreateMap<WardMaster, List<GetWardDto>>();

            CreateMap<CreateRoomDto, RoomMaster>();
            CreateMap<UpdateRoomDto, RoomMaster>();
            CreateMap<RoomMaster, GetRoomDto>();
            CreateMap<RoomMaster, List<GetRoomDto>>();

            CreateMap<CreateHospitalDto, HospitalMaster>();
            CreateMap<UpdateHospitalDto, HospitalMaster>();
            CreateMap<HospitalMaster, GetHospitalDto>();
            CreateMap<HospitalMaster, List<GetHospitalDto>>();

            CreateMap<CreateHospitalBranchDto, BranchMaster>();
            CreateMap<UpdateHospitalBranchDto, BranchMaster>();
            CreateMap<BranchMaster, GetHospitalBranchDto>();
            CreateMap<BranchMaster, List<GetHospitalBranchDto>>();

            CreateMap<CreateDoctorDto, DoctorMaster>();
            CreateMap<UpdateDoctorDto, DoctorMaster>();
            CreateMap<DoctorMaster, GetDoctorDto>();
            CreateMap<DoctorMaster, List<GetDoctorDto>>();

            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();
            CreateMap<Appointment, GetAppointmentDto>();
            CreateMap<Appointment, List<GetAppointmentDto>>();

            CreateMap<CreatePatientPrescriptionDto, PatientPrescription>();
            CreateMap<UpdatePatientPrescriptionDto, PatientPrescription>();
            CreateMap<PatientPrescription, GetPatientPrescriptionDto>();
            CreateMap<PatientPrescription, List<GetPatientPrescriptionDto>>();

            CreateMap<CreatePrescriptionBillingDto, PrescriptionBilling>();
            CreateMap<PrescriptionBilling, GetPrescriptionBillingDto>();

            CreateMap<DashboardData, DashboardDataDto>();
        }
    }
}
