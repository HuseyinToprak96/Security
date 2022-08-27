using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataProjection.Models;
using Microsoft.AspNetCore.DataProtection;

namespace DataProjection.Controllers
{
    public class UrunlersController : Controller
    {
        private readonly DENEMEContext _context;
        private readonly IDataProtector _dataProtector;
        public UrunlersController(DENEMEContext context, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector("ProductController");//burda verilen isim dataProtector lar arasındaki farkı sağlar.Aynı isim verilmemesi gerekir.
        }

        // GET: Urunlers
        public async Task<IActionResult> Index()
        {
            var products = await _context.Urunlers.ToListAsync();

            var timeLimitedDataProtector=_dataProtector.ToTimeLimitedDataProtector();
            products.ForEach(x =>
            {
                //   x.EncrypedId = _dataProtector.Protect(x.Id.ToString());
                x.EncrypedId =timeLimitedDataProtector.Protect(x.Id.ToString(),TimeSpan.FromSeconds(5));//şifreleme süresi vermek için kullanılır. açıklamalardaki kod süre vermeden çalıştırır.

            });
            return View(products);
        }
        [HttpPost]
        public IActionResult Index(string search)//'or '1'='1'-- gönderilirse arama kısmında bütün veriler gelir.
        {
           //kullanılırsa sqlinjection zaafiyetinden riskli olurdu. //var products = _context.Urunlers.FromSqlRaw($"select * from Urunler where Ad like '%{search}%'");
           //kullanılabilir. // var products = _context.Urunlers.FromSqlInterpolated($"select * from Urunler where Ad like '%{search}%'");            
            var products = _context.Urunlers.FromSqlRaw("select * from Urunler where Ad like '%{0}%'",search);

            return View(products);
        }
            // GET: Urunlers/Details/5
            public async Task<IActionResult> Details(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            //int decrypedId = int.Parse(_dataProtector.Unprotect(id));

            var timeLimitedDataProtector = _dataProtector.ToTimeLimitedDataProtector();

            int decrypedId = int.Parse(timeLimitedDataProtector.Unprotect(id));

            var urunler = await _context.Urunlers
                .FirstOrDefaultAsync(m => m.Id == decrypedId);
            if (urunler == null)
            {
                return NotFound();
            }

            return View(urunler);
        }

        // GET: Urunlers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Urunlers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,Fiyat,Stok,KategoriId,CreatedDate,UpdatedDate")] Urunler urunler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(urunler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(urunler);
        }

        // GET: Urunlers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urunler = await _context.Urunlers.FindAsync(id);
            if (urunler == null)
            {
                return NotFound();
            }
            return View(urunler);
        }

        // POST: Urunlers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Fiyat,Stok,KategoriId,CreatedDate,UpdatedDate")] Urunler urunler)
        {
            if (id != urunler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(urunler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrunlerExists(urunler.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(urunler);
        }

        // GET: Urunlers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urunler = await _context.Urunlers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urunler == null)
            {
                return NotFound();
            }

            return View(urunler);
        }

        // POST: Urunlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urunler = await _context.Urunlers.FindAsync(id);
            _context.Urunlers.Remove(urunler);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UrunlerExists(int id)
        {
            return _context.Urunlers.Any(e => e.Id == id);
        }
    }
}
