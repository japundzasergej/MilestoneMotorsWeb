using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWeb.Data.Enums;
using MilestoneMotorsWeb.Data.Interfaces;
using MilestoneMotorsWeb.Models;
using MilestoneMotorsWeb.Utilities;
using MilestoneMotorsWeb.ViewModels;
using X.PagedList;

namespace MilestoneMotorsWeb.Controllers
{
    public class HomeController(
        ICarsRepository carsRepository,
        IPhotoService photoService,
        ILogger<HomeController> logger
    ) : Controller
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly ILogger<HomeController> _logger = logger;

        private async Task<List<string>> CloudinaryUpload(List<IFormFile> files)
        {
            List<string> result =  [ ];
            foreach (var file in files)
            {
                if (file != null)
                {
                    var imageFile = await _photoService.AddPhotoAsync(file);
                    result.Add(imageFile.Url.ToString());
                }
            }
            return result;
        }

        private static string ConvertToEuro(double input)
        {
            return string.Format(new CultureInfo("de-De"), "{0:N0} €", input);
        }

        private static EditCarViewModel EditVmMapper(Car source)
        {
            var capacity = source.EngineCapacity.Split(" ");
            var mileage = source.Mileage.Split(" ");
            var enginePower = source.EnginePower.Split(" ");
            return new EditCarViewModel
            {
                UserId = source.UserId,
                Condition = source.Condition,
                Brand = source.Brand,
                Description = source.Description,
                Price = default,
                Model = source.Model,
                ManufacturingYear = source.ManufacturingYear,
                Mileage = mileage[0],
                BodyTypes = source.BodyTypes,
                FuelTypes = source.FuelTypes,
                EngineCapacity = int.Parse(capacity[0]),
                EnginePower = enginePower[0],
                FixedPrice = source.FixedPrice,
                Exchange = source.Exchange
            };
        }

        private void MessageSent()
        {
            TempData["Success"] = "Message sent!";
        }

        private static int ConvertPrice(string price)
        {
            string[] parts = price.Split(' ');

            if (int.TryParse(parts[0], out int numericPart))
            {
                return numericPart;
            }
            return 0;
        }

        private static List<Car> ApplyOrdering(List<Car> source, string order)
        {
            return order switch
            {
                "priceDesc" => [ ..source.OrderByDescending(c => ConvertPrice(c.Price)) ],
                "priceAsc" => [ .. source.OrderBy(c => ConvertPrice(c.Price)) ],
                "yearDesc" => [ .. source.OrderByDescending(c => c.ManufacturingYear) ],
                _ => source,
            };
        }

        private static List<Car> ApplyFuelTypeFilter(List<Car> source, string fuelTypeFilter)
        {
            if (!string.IsNullOrEmpty(fuelTypeFilter))
            {
                var selectedFuelType = Enum.Parse<FuelTypes>(fuelTypeFilter);
                source = source.Where(c => c.FuelTypes == selectedFuelType).ToList();
            }
            return source;
        }

        private static List<Car> ApplyConditionFilter(List<Car> source, string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                var selectedCondition = Enum.Parse<Condition>(condition);
                source = source.Where(c => c.Condition == selectedCondition).ToList();
            }
            return source;
        }

        private static List<Car> ApplyBrandFilter(List<Car> source, string brandFilter)
        {
            if (!string.IsNullOrEmpty(brandFilter))
            {
                var selectedBrand = Enum.Parse<Brand>(brandFilter);
                source = source.Where(c => c.Brand == selectedBrand).ToList();
            }
            return source;
        }

        public async Task<IActionResult> Index(
            string search,
            string orderBy,
            string fuelTypeFilter,
            string conditionFilter,
            string brandFilter,
            int? page
        )
        {
            ViewBag.Search = search;
            ViewBag.OrderBy = orderBy;
            ViewBag.FuelTypeFilter = fuelTypeFilter;
            ViewBag.BodyTypeFilter = conditionFilter;
            ViewBag.BrandFilter = brandFilter;
            var carsList = await _carsRepository.GetAllCarsAsync();

            var searchedList = carsList
                .Where(
                    c =>
                        (
                            string.IsNullOrWhiteSpace(search)
                            || c.Brand
                                .ToString()
                                .StartsWith(search, StringComparison.OrdinalIgnoreCase)
                            || c.Model.StartsWith(search, StringComparison.OrdinalIgnoreCase)
                        )
                )
                .ToList();

            searchedList = ApplyOrdering(searchedList, orderBy);
            searchedList = ApplyFuelTypeFilter(searchedList, fuelTypeFilter);
            searchedList = ApplyConditionFilter(searchedList, conditionFilter);
            searchedList = ApplyBrandFilter(searchedList, brandFilter);

            if (searchedList.Count == 0)
            {
                return View(new List<Car>().ToPagedList());
            }

            int pageSize = 6;
            int pageNumber = page ?? 1;

            var pagedList = searchedList.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var carDetail = await _carsRepository.GetCarByIdAsync(id);

            if (carDetail == null)
            {
                return NotFound();
            }
            return View(carDetail);
        }

        [Authorize]
        public IActionResult Create()
        {
            var currentUserId = HttpContext.User.GetUserId();
            var carVM = new CreateCarViewModel { UserId = currentUserId };
            return View(carVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCarViewModel carVM)
        {
            if (ModelState.IsValid)
            {
                var headlinerImage = await _photoService.AddPhotoAsync(carVM.HeadlinerImageUrl);
                List<IFormFile> files =
                [
                    carVM.PhotoOne,
                    carVM.PhotoTwo,
                    carVM.PhotoThree,
                    carVM.PhotoFour,
                    carVM.PhotoFive,
                ];
                var imageList = await CloudinaryUpload(files);
                var carObject = new Car()
                {
                    Condition = carVM.Condition,
                    Brand = carVM.Brand,
                    Description = carVM.Description.FirstCharToUpper().Trim(),
                    Price = ConvertToEuro(carVM.Price),
                    Model = carVM.Model.FirstCharToUpper().Trim(),
                    ManufacturingYear = carVM.ManufacturingYear,
                    Mileage = carVM.Mileage.ToString() + " (km)",
                    BodyTypes = carVM.BodyTypes,
                    FuelTypes = carVM.FuelTypes,
                    EngineCapacity = carVM.EngineCapacity.ToString() + " (cm3)",
                    EnginePower = carVM.EnginePower + " (kW/HP)",
                    FixedPrice = carVM.FixedPrice,
                    Exchange = carVM.Exchange,
                    HeadlinerImageUrl = headlinerImage.Url.ToString(),
                    ImagesUrl = imageList,
                    UserId = carVM.UserId,
                    AdNumber = String.Concat(carVM.UserId, "-", carVM.Brand, "-", carVM.Model),
                    CreatedAt = DateTime.UtcNow,
                };
                await _carsRepository.Add(carObject);
                TempData["Success"] = "Listing created successfully!";
                return RedirectToAction("Index");
            }
            return View(carVM);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var userCar = await _carsRepository.GetCarByIdAsync(id);
            if (userCar == null)
            {
                return NotFound();
            }
            var carVm = EditVmMapper(userCar);
            return View(carVm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EditCarViewModel carViewModel)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var userCar = await _carsRepository.GetCarByIdNoTrackAsync(id);
            if (ModelState.IsValid)
            {
                if (userCar != null)
                {
                    var car = new Car
                    {
                        Id = (int)id,
                        UserId = userCar.UserId,
                        Condition = carViewModel.Condition,
                        Brand = carViewModel.Brand,
                        Description = carViewModel.Description.FirstCharToUpper().Trim(),
                        Model = carViewModel.Model.FirstCharToUpper().Trim(),
                        Price = ConvertToEuro(carViewModel.Price),
                        ManufacturingYear = carViewModel.ManufacturingYear,
                        Mileage = carViewModel.Mileage + " (km)",
                        BodyTypes = carViewModel.BodyTypes,
                        FuelTypes = carViewModel.FuelTypes,
                        EngineCapacity = carViewModel.EngineCapacity.ToString() + " (cm3)",
                        EnginePower = carViewModel.EnginePower + " (kW/hP)",
                        FixedPrice = carViewModel.FixedPrice,
                        Exchange = carViewModel.Exchange,
                        HeadlinerImageUrl = userCar.HeadlinerImageUrl,
                        ImagesUrl = userCar.ImagesUrl,
                        CreatedAt = userCar.CreatedAt,
                        AdNumber = userCar.AdNumber,
                    };
                    await _carsRepository.Update(car);
                    TempData["Success"] = "Listing updated successfully!";
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return View(carViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var userCar = await _carsRepository.GetCarByIdAsync(id);

            if (userCar == null)
            {
                return NotFound();
            }
            await _carsRepository.Remove(userCar);
            TempData["Success"] = "Listing deleted successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult SendMessage()
        {
            MessageSent();
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(
                    new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                    }
                );
            }
        }
    }
}
