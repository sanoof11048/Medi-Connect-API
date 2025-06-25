using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.Models.Admin;
using Medi_Connect.Domain.Models.ApiResponses;

namespace Medi_Connect.Application.Services
{
    public class CareTypeRateService : ICareTypeRateService
    {
        private readonly ICareTypeRateRepository _repo;
        private readonly IMapper _mapper;

        public CareTypeRateService(ICareTypeRateRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<CareTypeRateResponseDTO>>> GetAllAsync()
        {
            try
            {
                var list = await _repo.GetAllAsync();
                var all = _mapper.Map<List<CareTypeRateResponseDTO>>(list);
                return new ApiResponse<List<CareTypeRateResponseDTO>>(200, "Fetched", all);
            }
            catch (Exception)
            {
                return new ApiResponse<List<CareTypeRateResponseDTO>>(500, "Something went wrong", null);
            }
        }

        public async Task<ApiResponse<CareTypeRateResponseDTO?>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<CareTypeRateResponseDTO?>(404, "Not found", null);

                var dto = _mapper.Map<CareTypeRateResponseDTO>(entity);
                return new ApiResponse<CareTypeRateResponseDTO?>(200, "Fetched", dto);
            }
            catch (Exception)
            {
                return new ApiResponse<CareTypeRateResponseDTO?>(500, "Something went wrong", null);
            }
        }

        public async Task<ApiResponse<string>> CreateAsync(CareTypeRateCreateDTO dto)
        {
            try
            {
                var exists = await _repo.GetByTypeAsync(dto.ServiceType);
                if (exists != null)
                    return new ApiResponse<string>(409, "This CareType already has a rate.");

                if (dto.OfferPrice.HasValue && dto.OfferPrice >= dto.FixedPayment)
                {
                    return new ApiResponse<string>(400, "Offer price must be less than fixed price.");
                }

                var entity = _mapper.Map<CareTypeRate>(dto);
                entity.Id = Guid.NewGuid();
                await _repo.AddAsync(entity);

                return new ApiResponse<string>(200, "Rate added successfully.");
            }
            catch (Exception)
            {
                return new ApiResponse<string>(500, "Something went wrong.");
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(CareTypeRateUpdateDTO dto)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(dto.Id);
                if (existing == null)
                    return new ApiResponse<string>(404, "Rate not found");

                if (dto.OfferPrice.HasValue && dto.OfferPrice >= dto.FixedPayment)
                {
                    return new ApiResponse<string>(400, "Offer price must be less than fixed price.");
                }

                if (existing.ServiceType != dto.ServiceType)
                {
                    var duplicate = await _repo.GetByTypeAsync(dto.ServiceType);
                    if (duplicate != null)
                        return new ApiResponse<string>(409, "Another rate with this CareType already exists.");
                }

                existing.FixedPayment = dto.FixedPayment;
                existing.ServiceType = dto.ServiceType;
                existing.OfferPrice = dto.OfferPrice;
                existing.Description = dto.Description;

                await _repo.UpdateAsync(existing);
                return new ApiResponse<string>(200, "Rate updated");
            }
            catch (Exception)
            {
                return new ApiResponse<string>(500, "Something went wrong.");
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<string>(404, "Rate not found");

                await _repo.DeleteAsync(id);
                return new ApiResponse<string>(200, "Rate deleted");
            }
            catch (Exception)
            {
                return new ApiResponse<string>(500, "Something went wrong.");
            }
        }
    }
}
