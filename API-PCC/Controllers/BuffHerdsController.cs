using API_PCC.ApplicationModels;
using API_PCC.ApplicationModels.Common;
using API_PCC.Data;
using API_PCC.DtoModels;
using API_PCC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static API_PCC.Manager.DBMethods;
using System;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class HBuffHerdsController : ControllerBase
    {
        private readonly PCC_DEVContext _context;

        public HBuffHerdsController(PCC_DEVContext context)
        {
            _context = context;
        }

        // POST: BuffHerds/list
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HBuffHerd>>> list(BuffHerdSearchFilterModel searchFilter)
        {
          if (_context.HBuffHerds == null)
          {
              return NotFound();
            }

            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;


            var hBuffHerdList = _context.HBuffHerds.AsNoTracking();
            hBuffHerdList = hBuffHerdList.Where(buffHerd => !buffHerd.DeleteFlag);
            try
            {
                if (searchFilter.herdCode != null && searchFilter.herdCode != "")
                {
                    hBuffHerdList = hBuffHerdList.Where(buffHerd => buffHerd.HerdCode.Contains(searchFilter.herdCode));
                }

                if (searchFilter.herdName != null && searchFilter.herdName != "")
                {
                    hBuffHerdList = hBuffHerdList.Where(buffHerd => buffHerd.HerdName.Contains(searchFilter.herdName));
                }

                if (searchFilter.ownerName != null && searchFilter.ownerName != "")
                {
                    hBuffHerdList = hBuffHerdList.Where(buffHerd => buffHerd.Owner.Contains(searchFilter.ownerName));
                }

                if (searchFilter.farmManager != null && searchFilter.farmManager != "")
                {
                    hBuffHerdList = hBuffHerdList.Where(buffHerd => buffHerd.FarmManager.Contains(searchFilter.farmManager));
                }

                totalItems = hBuffHerdList.ToList().Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                items = hBuffHerdList.Skip((page - 1) * pagesize).Take(pagesize).ToList();

                var result = new List<HerdPagedModel>();
                var item = new HerdPagedModel();

                int pages = searchFilter.page == 0 ? 1 : searchFilter.page;
                item.CurrentPage = searchFilter.page == 0 ? "1" : searchFilter.page.ToString();
                int page_prev = pages - 1;

                double t_records = Math.Ceiling(Convert.ToDouble(totalItems) / Convert.ToDouble(pagesize));
                int page_next = searchFilter.page >= t_records ? 0 : pages + 1;
                item.NextPage = items.Count % pagesize >= 0 ? page_next.ToString() : "0";
                item.PrevPage = pages == 1 ? "0" : page_prev.ToString();
                item.TotalPage = t_records.ToString();
                item.PageSize = pagesize.ToString();
                item.TotalRecord = totalItems.ToString();
                item.items = items;
                result.Add(item);
                return Ok(result);
            }

            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // GET: BuffHerds/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HBuffHerd>> search(int id)
        {
            if (_context.HBuffHerds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BuffHerd' is null!");
            }
            var hBuffHerd = await _context.HBuffHerds.FindAsync(id);

            if (hBuffHerd == null || hBuffHerd.DeleteFlag)
            {
                return Conflict("No records found!");
            }

            return Ok(hBuffHerd);

        }

        // PUT: BuffHerds/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, BuffHerdUpdateModel registrationModel)
        {
            if (_context.HBuffHerds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HBuffHerds' is null!");
            }

            var buffHerd = _context.HBuffHerds.AsNoTracking().Where(buffHerd => !buffHerd.DeleteFlag && buffHerd.Id == id).FirstOrDefault();

            if (buffHerd == null)
            {
                return Conflict("No records matched!");
            }

            bool hasDuplicateOnUpdate = (_context.HBuffHerds?.Any(buffHerd => !buffHerd.DeleteFlag && buffHerd.HerdName == registrationModel.HerdName && buffHerd.HerdCode == registrationModel.HerdCode && buffHerd.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                buffHerd = populateBuffHerd(buffHerd, registrationModel);
                buffHerd.HerdSize = registrationModel.HerdSize;
                buffHerd.DateUpdated = DateTime.Now;
                buffHerd.UpdatedBy = registrationModel.UpdatedBy;

                _context.Entry(buffHerd).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        private HBuffHerd populateBuffHerd(HBuffHerd buffHerd, BuffHerdUpdateModel updateModel)
        {

            if (updateModel.HerdName != null && updateModel.HerdName != "")
            {
                buffHerd.HerdName = updateModel.HerdName;
            }
            if (updateModel.HerdCode != null && updateModel.HerdCode != "")
            {
                buffHerd.HerdCode = updateModel.HerdCode;
            }
            if (updateModel.BBuffCode != null && updateModel.BBuffCode != "")
            {
                buffHerd.BBuffCode = updateModel.BBuffCode;
            }
            if (updateModel.FCode != null && updateModel.FCode != "")
            {
                buffHerd.FCode = updateModel.FCode;
            }
            if (updateModel.HTypeCode != null && updateModel.HTypeCode != "")
            {
                buffHerd.HTypeCode = updateModel.HTypeCode;
            }
            if (updateModel.FeedCode != null && updateModel.FeedCode != "")
            {
                buffHerd.FeedCode = updateModel.FeedCode;
            }
            if (updateModel.FarmManager != null && updateModel.FarmManager != "")
            {
                buffHerd.FarmManager = updateModel.FarmManager;
            }
            if (updateModel.FarmAddress != null && updateModel.FarmAddress != "")
            {
                buffHerd.FarmAddress = updateModel.FarmAddress;
            }
            if (updateModel.Owner != null && updateModel.Owner != "")
            {
                buffHerd.Owner = updateModel.Owner;
            }
            if (updateModel.Address != null && updateModel.Address != "")
            {
                buffHerd.Address = updateModel.Address;
            }
            if (updateModel.TelNo != null && updateModel.TelNo != "")
            {
                buffHerd.TelNo = updateModel.TelNo;
            }
            if (updateModel.MNo != null && updateModel.MNo != "")
            {
                buffHerd.MNo = updateModel.MNo;
            }
            if (updateModel.Email != null && updateModel.Email != "")
            {
                buffHerd.Email = updateModel.Email;
            }
            if (updateModel.OrganizationName != null && updateModel.OrganizationName != "")
            {
                buffHerd.OrganizationName = updateModel.OrganizationName;
            }
            return buffHerd;

        }
        // POST: BuffHerds/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HBuffHerd>> save(BuffHerdRegistrationModel registrationModel)
        {
          if (_context.HBuffHerds == null)
          {
              return Problem("Entity set 'PCC_DEVContext.HBuffHerds'  is null.");
          }

            bool hasDuplicateOnSave = (_context.HBuffHerds?.Any(buffHerd => !buffHerd.DeleteFlag && (buffHerd.HerdCode == registrationModel.HerdCode || buffHerd.HerdName == registrationModel.HerdName))).GetValueOrDefault();

            if (hasDuplicateOnSave)
            {
                return Conflict("Herd already exists");
            }

            try
            {
                var BuffHerdModel = buildBuffHerd(registrationModel);
                BuffHerdModel.DateCreated = DateTime.Now;

                _context.HBuffHerds.Add(BuffHerdModel);
                await _context.SaveChangesAsync();

                return Ok("Herd successfully registered!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        private HBuffHerd buildBuffHerd(BuffHerdRegistrationModel registrationModel)
        {
            var BuffHerdModel = new HBuffHerd()
            {
                HerdName = registrationModel.HerdName,
                HerdCode = registrationModel.HerdCode,
                HerdSize = registrationModel.HerdSize,
                BBuffCode = registrationModel.BBuffCode,
                FCode = registrationModel.FCode,
                HTypeCode = registrationModel.HTypeCode,
                FeedCode = registrationModel.FeedCode,
                FarmManager = registrationModel.FarmManager,
                FarmAddress = registrationModel.FarmAddress,
                Owner = registrationModel.Owner,
                Address = registrationModel.Address,
                TelNo = registrationModel.TelNo,
                MNo = registrationModel.MNo,
                Email = registrationModel.Email,
                OrganizationName = registrationModel.OrganizationName,
                CreatedBy = registrationModel.CreatedBy,
            };

            return BuffHerdModel;
        }

        // DELETE: BuffHerds/delete/5
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {
            if (_context.HBuffHerds == null)
            {
                return NotFound();
            }
            var hBuffHerd = await _context.HBuffHerds.FindAsync(deletionModel.id);
            if (hBuffHerd == null || hBuffHerd.DeleteFlag)
            {
                return Conflict("No records matched!");
            }

            try
            {
                hBuffHerd.DeleteFlag = true;
                hBuffHerd.DateDeleted = DateTime.Now;
                hBuffHerd.DeletedBy = deletionModel.deletedBy;
                hBuffHerd.DateRestored = null;
                hBuffHerd.RestoredBy = "";
                _context.Entry(hBuffHerd).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Deletion Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: BuffHerds/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {

            if (_context.HBuffHerds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BuffHerd' is null!");
            }

            var hBuffHerd = await _context.HBuffHerds.FindAsync(restorationModel.id);
            if (hBuffHerd == null || !hBuffHerd.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                hBuffHerd.DeleteFlag = !hBuffHerd.DeleteFlag;
                hBuffHerd.DateDeleted = null;
                hBuffHerd.DeletedBy = "";
                hBuffHerd.DateRestored = DateTime.Now;
                hBuffHerd.RestoredBy = restorationModel.restoredBy;

                _context.Entry(hBuffHerd).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        private bool HBuffHerdExists(int id)
        {
            return (_context.HBuffHerds?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
