using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Validations
{
    public class RoomValidation : AbstractValidator<CreateRoomDto>
    {
        public RoomValidation()
        {
            RuleFor(x => x.RoomName)
               .NotEmpty().WithMessage(RoomConst.RoomNameBlankError)
               .MaximumLength(RoomConst.RoomNameLength);

            RuleFor(x => x.RoomDescription)
              .NotEmpty().WithMessage(RoomConst.DescriptionLengthError)
              .MaximumLength(RoomConst.DescriptionLength);
        }
    }

    public class UpdateRoomValidation : AbstractValidator<UpdateRoomDto>
    {
        public UpdateRoomValidation()
        {
            RuleFor(x => x.RoomName)
               .NotEmpty().WithMessage(RoomConst.RoomNameBlankError)
               .MaximumLength(RoomConst.RoomNameLength);

            RuleFor(x => x.RoomDescription)
              .NotEmpty().WithMessage(RoomConst.DescriptionLengthError)
              .MaximumLength(RoomConst.DescriptionLength);
        }
    }
}
