using System;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWeb.Data.Enums;
using MilestoneMotorsWeb.Models;
using MilestoneMotorsWeb.Utilities;

namespace MilestoneMotorsWeb.Data
{
    public class CarSeeder
    {
        private static readonly Random random = new(Guid.NewGuid().GetHashCode());

        public static List<Car> SeedCars(string[] uniqueUserIds)
        {
            List<Car> cars =  [ ];

            for (int i = 0; i < 20; i++)
            {
                Condition condition = GetRandomCondition();
                Brand brand = GetAllBrands(i);
                string model = GetModelByBrand(brand);
                string description = GetRandomDescription(
                    condition.ToString(),
                    brand.ToString(),
                    model,
                    i
                );
                string price = GetRandomPrice(brand, condition);
                int manufacturingYear = GetPlausibleManufacturingYear(condition);
                string mileage = GetPlausibleMileage(condition);
                BodyTypes bodyType = GetBodyTypeByModel(model);
                FuelTypes fuelType = GetFuelTypeByModel(model);
                string engineCapacity = GetPlausibleEngineCapacity(model);
                string enginePower = GetPlausibleEnginePower();
                YesOrNo fixedPrice = GetRandomYesOrNo();
                YesOrNo exchange = GetRandomYesOrNo();
                string headlinerImageUrl = GetHeadlinerByModel(model);
                List<string> imagesUrl = GetImagesByModel(model);
                DateTime createdAt = DateTime.UtcNow.AddDays(-random.Next(1, 30));

                cars.Add(
                    new Car
                    {
                        UserId = uniqueUserIds[i],
                        Condition = condition,
                        Brand = brand,
                        Model = model,
                        Description = description,
                        Price = price,
                        ManufacturingYear = manufacturingYear,
                        Mileage = mileage,
                        BodyTypes = bodyType,
                        FuelTypes = fuelType,
                        EngineCapacity = engineCapacity,
                        EnginePower = enginePower,
                        FixedPrice = fixedPrice,
                        Exchange = exchange,
                        HeadlinerImageUrl = headlinerImageUrl,
                        ImagesUrl = imagesUrl,
                        CreatedAt = createdAt,
                        AdNumber = uniqueUserIds[i] + "-" + model
                    }
                );
            }

            return cars;
        }

        private static string GetRandomDescription(
            string condition,
            string brand,
            string model,
            int index
        )
        {
            string[] firstParts =
            {
                "I'm selling a",
                "Check out this awesome",
                "Looking to part ways with my",
                "For sale: A well-maintained",
                "Don't miss out on this fantastic",
                "Selling my reliable",
                "Up for grabs - a sleek",
                "Considering selling my trusty",
                "Time to say goodbye to my beloved",
                "Looking for a new home for my",
                "Thinking of selling my cherished",
                "Presenting the one and only",
                "For sale: A pristine",
                "Parting with my incredible",
                "Offering for sale a gorgeous",
                "Check this out - a stunning",
                "Selling a stylish",
                "Upgrading, so selling my",
                "Ready to part with my classy",
                "Time to sell my iconic"
            };

            string[] secondParts =
            {
                "in excellent condition!",
                "you won't be disappointed!",
                "runs like a dream!",
                "well taken care of!",
                "low mileage and well-maintained!",
                "great fuel efficiency!",
                "perfect for daily commuting!",
                "smooth and reliable!",
                "loaded with features!",
                "must see to appreciate!",
                "fantastic performance!",
                "recently serviced!",
                "clean title!",
                "sleek and stylish!",
                "luxurious and comfortable!",
                "great for road trips!",
                "ready for a new adventure!",
                "sporty and fun to drive!",
                "perfect family car!",
                "built for comfort and style!"
            };
            return $"{firstParts[index]} {condition.ToLower()} {brand} {model} {secondParts[index]}";
        }

        private static Condition GetRandomCondition()
        {
            return random.Next(2) == 0 ? Condition.Used : Condition.New;
        }

        private static Brand GetAllBrands(int i)
        {
            Array values = Enum.GetValues(typeof(Brand));

            if (i >= 0 && i < values.Length)
            {
                return (Brand)values.GetValue(i);
            }
            else
            {
                return (Brand)values.GetValue(0);
            }
        }

        private static string GetModelByBrand(Brand brand)
        {
            return brand switch
            {
                Brand.Audi => "A5",
                Brand.Volkswagen => "Jetta",
                Brand.Ford => "F-150",
                Brand.Chevrolet => "Camaro",
                Brand.Honda => "Civic",
                Brand.Nissan => "Altima",
                Brand.BMW => "X5",
                Brand.Mercedes => "C-Class",
                Brand.Hyundai => "Elantra",
                Brand.Kia => "Soul",
                Brand.Subaru => "Outback",
                Brand.Tesla => "Model 3",
                Brand.Porsche => "911",
                Brand.Jaguar => "F-PACE",
                Brand.Mazda => "MX-5",
                Brand.Volvo => "S60",
                Brand.Fiat => "500",
                Brand.Jeep => "Wrangler",
                _ => "Q5",
            };
        }

        private static bool IsExpensiveBrand(Brand brand)
        {
            return brand switch
            {
                Brand.Audi
                or Brand.Chevrolet
                or Brand.Mercedes
                or Brand.BMW
                or Brand.Tesla
                or Brand.Porsche
                or Brand.Jaguar
                or Brand.Jeep
                    => true,
                _ => false
            };
        }

        private static double GetEstimatedNewPrice(Brand brand)
        {
            return brand switch
            {
                Brand.Audi => 60000,
                Brand.Volkswagen => 25000,
                Brand.Ford => 35000,
                Brand.Chevrolet => 30000,
                Brand.Honda => 28000,
                Brand.Nissan => 26000,
                Brand.BMW => 70000,
                Brand.Mercedes => 65000,
                Brand.Hyundai => 23000,
                Brand.Kia => 22000,
                Brand.Subaru => 27000,
                Brand.Tesla => 75000,
                Brand.Porsche => 90000,
                Brand.Jaguar => 80000,
                Brand.Mazda => 32000,
                Brand.Volvo => 40000,
                Brand.Fiat => 18000,
                Brand.Jeep => 35000,
                _ => (double)28000,
            };
        }

        private static double GetEstimatedUsedPrice(Brand brand)
        {
            return brand switch
            {
                Brand.Audi => 45000,
                Brand.Volkswagen => 15000,
                Brand.Ford => 20000,
                Brand.Chevrolet => 18000,
                Brand.Honda => 16000,
                Brand.Nissan => 14000,
                Brand.BMW => 45000,
                Brand.Mercedes => 42000,
                Brand.Hyundai => 15000,
                Brand.Kia => 14000,
                Brand.Subaru => 18000,
                Brand.Tesla => 60000,
                Brand.Porsche => 70000,
                Brand.Jaguar => 60000,
                Brand.Mazda => 22000,
                Brand.Volvo => 28000,
                Brand.Fiat => 10000,
                Brand.Jeep => 20000,
                _ => (double)17500
            };
        }

        private static string GetRandomPrice(Brand brand, Condition condition)
        {
            if (condition == Condition.New)
            {
                double estimatedPrice = GetEstimatedNewPrice(brand);
                var isExpensive = IsExpensiveBrand(brand);

                return isExpensive
                    ? ConvertToEuroMethod.ConvertToEuro(estimatedPrice + random.Next(5000, 10000))
                    : ConvertToEuroMethod.ConvertToEuro(estimatedPrice + random.Next(2500, 3500));
            }
            else
            {
                double estimatedPrice = GetEstimatedUsedPrice(brand);
                var isExpensive = IsExpensiveBrand(brand);

                return isExpensive
                    ? ConvertToEuroMethod.ConvertToEuro(estimatedPrice - random.Next(2500, 5000))
                    : ConvertToEuroMethod.ConvertToEuro(estimatedPrice - random.Next(1500, 2500));
            }
        }

        private static int GetPlausibleManufacturingYear(Condition condition)
        {
            int currentYear = DateTime.UtcNow.Year;
            int maxYear =
                condition == Condition.New
                    ? currentYear - random.Next(1, 5)
                    : currentYear - random.Next(3, 10);
            return random.Next(maxYear - 10, maxYear + 1);
        }

        private static string GetPlausibleMileage(Condition condition)
        {
            var randomNew = random.Next(1000, 25000);
            return condition == Condition.New
                ? $"{randomNew} km"
                : $"{random.Next(10000, 80000)} km";
        }

        private static BodyTypes GetBodyTypeByModel(string model)
        {
            return model switch
            {
                "A5" => BodyTypes.Coupe,
                "Jetta" => BodyTypes.Sedan,
                "F-150" => BodyTypes.Pickup,
                "Camaro" => BodyTypes.Coupe,
                "Civic" => BodyTypes.Sedan,
                "Altima" => BodyTypes.Sedan,
                "X5" => BodyTypes.Suv,
                "C-Class" => BodyTypes.Sedan,
                "Q5" => BodyTypes.Suv,
                "Elantra" => BodyTypes.Sedan,
                "Soul" => BodyTypes.Hatchback,
                "Outback" => BodyTypes.StationWagon,
                "Model 3" => BodyTypes.Sedan,
                "911" => BodyTypes.Coupe,
                "F-PACE" => BodyTypes.Suv,
                "MX-5" => BodyTypes.Roadster,
                "S60" => BodyTypes.Sedan,
                "500" => BodyTypes.Hatchback,
                "Wrangler" => BodyTypes.Suv,
                _ => BodyTypes.Compact
            };
        }

        private static FuelTypes GetFuelTypeByModel(string model)
        {
            return model switch
            {
                "A5" or "Jetta" or "F-150" or "Camaro" or "Civic" => FuelTypes.Gasoline,
                "Altima"
                or "X5"
                or "C-Class"
                or "Q5"
                or "Elantra"
                or "Soul"
                or "Outback"
                    => FuelTypes.Diesel,
                "Model 3" or "911" or "F-PACE" or "MX-5" or "S60" => FuelTypes.Electric,
                "500" or "Wrangler" => FuelTypes.Hybrid,
                _ => FuelTypes.Gasoline,
            };
        }

        private static string GetPlausibleEngineCapacity(string model)
        {
            return model switch
            {
                "A5" => "1968 (cm³)",
                "Jetta" => "2000 (cm³)",
                "F-150" => "5000 (cm³)",
                "Camaro" => "3600 (cm³)",
                "Civic" => "2000 (cm³)",
                "Altima" => "2500 (cm³)",
                "X5" => "4400 (cm³)",
                "C-Class" => "3000 (cm³)",
                "Q5" => "3000 (cm³)",
                "Elantra" => "2000 (cm³)",
                "Soul" => "1600 (cm³)",
                "Outback" => "3600 (cm³)",
                "Model 3" => "Electric (no cm³)",
                "911" => "3800 (cm³)",
                "F-PACE" => "3000 (cm³)",
                "MX-5" => "2000 (cm³)",
                "S60" => "2500 (cm³)",
                "500" => "900 (cm³)",
                "Wrangler" => "3600 (cm³)",
                _ => "2000 (cm³)"
            };
        }

        private static string GetPlausibleEnginePower()
        {
            return $"{random.Next(100, 300)}/{random.Next(100, 300)} (kW/HP)";
        }

        private static YesOrNo GetRandomYesOrNo()
        {
            return random.Next(2) == 0 ? YesOrNo.YES : YesOrNo.NO;
        }

        private static string GetHeadlinerByModel(string model)
        {
            return model switch
            {
                "A5"
                    => "https://prod.pictures.autoscout24.net/listing-images/8769c1f5-ef7c-40f0-a1f4-ce7c4ccff913_80eef870-dd35-4041-bd92-2243da911610.jpg/1080x810.webp",
                "Jetta"
                    => "https://prod.pictures.autoscout24.net/listing-images/7ade05ce-7c7d-4511-bdb7-ca93e8f8fe78_abcbcf57-a0b5-4c92-9cda-6d7f03066cf8.jpg/1080x810.webp",
                "Camaro"
                    => "https://prod.pictures.autoscout24.net/listing-images/037df248-2c46-4510-abd7-5bfe40ca0baf_ec60e7c2-1b48-41dc-be81-d32352d0496a.jpg/1080x810.webp",
                "Civic"
                    => "https://prod.pictures.autoscout24.net/listing-images/0ae0492b-b3dd-42c0-b3b8-a511da0a71e6_85244fd8-d7e7-43b2-b458-499f84427dee.jpg/1080x810.webp",
                "Altima"
                    => "https://www.edmunds.com/assets/m/for-sale/a7-1n4bl4dw3mn311698/img-1-960x.jpg",
                "X5"
                    => "https://prod.pictures.autoscout24.net/listing-images/e9f16f35-66c1-4073-b9db-fc32136c72f8_367753fa-5098-4c87-834c-724dfb4bffa5.jpg/1080x810.webp",
                "C-Class"
                    => "https://content.homenetiol.com/2000096/2063606/640x480/c85e58b5c48d4fc2becac1aa947dce95.jpg",
                "Q5"
                    => "https://prod.pictures.autoscout24.net/listing-images/89c91eb4-4dc6-41c7-964d-9c2bfb3c762b_1d29a69a-b514-4304-820a-4148959d3ffc.jpg/1080x810.webp",
                "Elantra"
                    => "https://prod.pictures.autoscout24.net/listing-images/e9c5ee0f-f25e-4425-aa13-f00a579f5088_23d6712f-6e00-4995-b657-3c5f65ad0309.jpg/1080x810.webp",
                "Soul"
                    => "https://prod.pictures.autoscout24.net/listing-images/b14ad6ec-fd79-4691-a10b-aca1abdc775c_1c2f541e-0769-4d0f-a63c-5bdc108b0545.jpg/1080x810.webp",
                "Outback"
                    => "https://www.edmunds.com/assets/m/for-sale/ea-jf2gtabc5jh207085/img-1-960x.jpg",
                "Wrangler"
                    => "https://m.atcdn.co.uk/a/media/w1024h768/dd1b4198474a4dc69c38d368c374798b.jpg",
                "Model 3"
                    => "https://prod.pictures.autoscout24.net/listing-images/e08b698f-c0a5-4206-a858-2139c2bbeb84_1e52082e-8b70-4db3-8f30-48ee8a5e276b.jpg/1080x810.webp",
                "911"
                    => "https://prod.pictures.autoscout24.net/listing-images/b6f62328-e1b9-4b10-a684-20593c9b3a09_8242fb76-c1c4-4a41-a123-5eeb987ea4d1.jpg/1080x810.webp",
                "F-PACE"
                    => "https://prod.pictures.autoscout24.net/listing-images/d42617bc-b094-4d11-ba19-446488350b16_4f739321-e460-4b78-8bf5-9cd7b4e10e3b.jpg/1080x810.webp",
                "MX-5"
                    => "https://prod.pictures.autoscout24.net/listing-images/587134cd-6624-4fd9-9e13-0eef92a2ff58_9eca40df-d6f7-4852-ba4a-6fb4aba4c71d.jpg/1080x810.webp",
                "S60"
                    => "https://prod.pictures.autoscout24.net/listing-images/9df4fe82-ba8a-4bef-b802-5004b610d2ae_ad1be986-23bb-44c8-9d84-d124af466d90.jpg/1080x810.webp",
                "500"
                    => "https://prod.pictures.autoscout24.net/listing-images/66876487-73bc-41c3-b72a-3be7ccc9a466_f19e544b-1a61-4103-a50b-dfd7623e3845.jpg/1080x810.webp",
                "F-150"
                    => "https://prod.pictures.autoscout24.net/listing-images/90c8dc26-c946-4d80-8953-0902ca9a3c5a_00185442-f526-4a58-8cb8-71a53b1edf20.jpg/1080x810.webp",
                _ => "https://placehold.co/600x400?text=Milestone+Motors"
            };
        }

        private static List<string> GetImagesByModel(string model)
        {
            var a5Images = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/8769c1f5-ef7c-40f0-a1f4-ce7c4ccff913_3121c57d-83f5-4471-ae5e-938af6227c65.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/8769c1f5-ef7c-40f0-a1f4-ce7c4ccff913_331e7d98-5099-45c9-9b6c-0dda0748bd22.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/8769c1f5-ef7c-40f0-a1f4-ce7c4ccff913_5ce36d33-444d-47d6-b40c-125c15d7474d.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/8769c1f5-ef7c-40f0-a1f4-ce7c4ccff913_d0698c14-073b-4349-bd5f-c4dea9c0f779.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/8769c1f5-ef7c-40f0-a1f4-ce7c4ccff913_10f3c255-f5eb-4d6e-8b5c-e0e3652af050.jpg/1080x810.webp"
            };
            var jettaImages = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/7ade05ce-7c7d-4511-bdb7-ca93e8f8fe78_577e8268-f753-41b3-a6fe-dedc08cc4d17.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/7ade05ce-7c7d-4511-bdb7-ca93e8f8fe78_b8103ce4-6730-4a98-a12e-74eb91d907f2.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/7ade05ce-7c7d-4511-bdb7-ca93e8f8fe78_989c2833-5261-49ee-b44b-4ff5d58779ff.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/7ade05ce-7c7d-4511-bdb7-ca93e8f8fe78_e0fcc8a0-1d58-4937-bfcd-bcf26dace6f1.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/7ade05ce-7c7d-4511-bdb7-ca93e8f8fe78_20fc0e32-a7fe-495d-a09f-978fb623b683.jpg/1080x810.webp"
            };
            var camaroImages = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/037df248-2c46-4510-abd7-5bfe40ca0baf_79f148d1-ad4e-4be4-8be7-52d9f8debbbe.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/037df248-2c46-4510-abd7-5bfe40ca0baf_9e0f65cd-1865-4960-976a-cbfa37d41401.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/037df248-2c46-4510-abd7-5bfe40ca0baf_8f93e057-c6a4-4e18-9077-e80cf2c9cbf3.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/037df248-2c46-4510-abd7-5bfe40ca0baf_06ef03e2-0d56-4c34-b7cd-dee92dea7a40.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/037df248-2c46-4510-abd7-5bfe40ca0baf_0f204026-4e26-4990-a016-9a48fccdca7f.jpg/1080x810.webp"
            };

            var civicImages = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/0ae0492b-b3dd-42c0-b3b8-a511da0a71e6_40c4100f-14cf-4040-ba4e-e8ef61410541.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/0ae0492b-b3dd-42c0-b3b8-a511da0a71e6_64a407df-e669-4a70-b898-4dc7a002a89a.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/0ae0492b-b3dd-42c0-b3b8-a511da0a71e6_4b3cbff9-9553-4150-9007-0943d258cfbf.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/0ae0492b-b3dd-42c0-b3b8-a511da0a71e6_682bdeba-fe9f-45c1-a8f5-c178403c2d89.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/0ae0492b-b3dd-42c0-b3b8-a511da0a71e6_86dbdc85-a199-4b7e-9ba8-834c6b09277c.jpg/1080x810.webp"
            };

            var altimaImages = new List<string>
            {
                "https://www.edmunds.com/assets/m/for-sale/a7-1n4bl4dw3mn311698/img-1-960x.jpg",
                "https://www.edmunds.com/assets/m/for-sale/a7-1n4bl4dw3mn311698/img-2-960x.jpg",
                "https://www.edmunds.com/assets/m/for-sale/a7-1n4bl4dw3mn311698/img-3-960x.jpg",
                "https://www.edmunds.com/assets/m/for-sale/a7-1n4bl4dw3mn311698/img-4-960x.jpg",
                "https://www.edmunds.com/assets/m/for-sale/a7-1n4bl4dw3mn311698/img-5-960x.jpg",
                "https://www.edmunds.com/assets/m/for-sale/a7-1n4bl4dw3mn311698/img-6-960x.jpg"
            };

            var x5Images = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/e9f16f35-66c1-4073-b9db-fc32136c72f8_bc9ff68e-e140-4ab3-8045-5c47d9d1d24d.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e9f16f35-66c1-4073-b9db-fc32136c72f8_e6a158f0-7aba-4ea6-b0aa-a48c4ab4dd7d.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e9f16f35-66c1-4073-b9db-fc32136c72f8_0936c668-b930-44cc-a269-bfc78e2bdd05.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e9f16f35-66c1-4073-b9db-fc32136c72f8_8ec64075-2e46-415d-afb6-3caa48b97e3a.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e9f16f35-66c1-4073-b9db-fc32136c72f8_51225ab5-e015-4212-921b-402f024eb6e4.jpg/1080x810.webp"
            };

            var cClassImages = new List<string>
            {
                "https://content.homenetiol.com/2000096/2063606/640x480/b718912c926e456fa0178d1ece0705dd.jpg",
                "https://content.homenetiol.com/2000096/2063606/640x480/dae49449ed7041079c44e5d34a058075.jpg",
                "https://content.homenetiol.com/2000096/2063606/640x480/bcf4113fc19042d0bdaf024ca7564a8e.jpg",
                "https://content.homenetiol.com/2000096/2063606/640x480/e26f6e6b714c49568880f1b4762b38ad.jpg",
                "https://content.homenetiol.com/2000096/2063606/640x480/bee3952ced6d4aa0b4c4a1af0925349e.jpg"
            };

            var q5Images = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/89c91eb4-4dc6-41c7-964d-9c2bfb3c762b_f19358b6-03d1-4cb5-a95a-b58d32aef63f.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/89c91eb4-4dc6-41c7-964d-9c2bfb3c762b_1154ef0f-6499-4555-8eb4-86e30291bb92.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/89c91eb4-4dc6-41c7-964d-9c2bfb3c762b_c603cf15-90ae-4ebf-bff9-239fce5d9030.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/89c91eb4-4dc6-41c7-964d-9c2bfb3c762b_c603cf15-90ae-4ebf-bff9-239fce5d9030.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/89c91eb4-4dc6-41c7-964d-9c2bfb3c762b_772cf738-9438-4f9c-8c15-8a0a02003acd.jpg/1080x810.webp"
            };

            var elantraImages = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/e9c5ee0f-f25e-4425-aa13-f00a579f5088_e278cff4-1a87-4cc7-9afb-40af2bb7626f.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e9c5ee0f-f25e-4425-aa13-f00a579f5088_222acf01-b375-428e-ba89-78dc633dc1c2.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e9c5ee0f-f25e-4425-aa13-f00a579f5088_a55fe1e1-012b-4c9d-8ad8-725d344e9293.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e9c5ee0f-f25e-4425-aa13-f00a579f5088_83e343bd-1ca7-4c31-ae88-93645633bc8d.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e9c5ee0f-f25e-4425-aa13-f00a579f5088_83e343bd-1ca7-4c31-ae88-93645633bc8d.jpg/1080x810.webp"
            };

            var soulImages = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/b14ad6ec-fd79-4691-a10b-aca1abdc775c_7caaacc3-30e1-4648-a57a-6a5a70679cdd.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/b14ad6ec-fd79-4691-a10b-aca1abdc775c_43fb9837-12b7-4d09-9228-d2f95fa8c8f9.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/b14ad6ec-fd79-4691-a10b-aca1abdc775c_481621ba-e15e-45ba-aab1-2d839630f480.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/b14ad6ec-fd79-4691-a10b-aca1abdc775c_77f021ff-c43f-4f52-b8ad-3fd60e031ffb.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/b14ad6ec-fd79-4691-a10b-aca1abdc775c_132f5528-f4c4-4272-a07d-3047c6105fad.jpg/1080x810.webp"
            };

            var subaruImages = new List<string>
            {
                "https://www.edmunds.com/assets/m/for-sale/ea-jf2gtabc5jh207085/img-2-960x.jpg",
                "https://www.edmunds.com/assets/m/for-sale/ea-jf2gtabc5jh207085/img-3-960x.jpg",
                "https://www.edmunds.com/assets/m/for-sale/ea-jf2gtabc5jh207085/img-4-960x.jpg",
                "https://www.edmunds.com/assets/m/for-sale/ea-jf2gtabc5jh207085/img-5-960x.jpg",
                "https://www.edmunds.com/assets/m/for-sale/ea-jf2gtabc5jh207085/img-6-960x.jpg"
            };

            var wranglerImages = new List<string>
            {
                "https://m.atcdn.co.uk/a/media/w1024h768/66ed3de979c247dd840da2e32c9fca19.jpg",
                "https://m.atcdn.co.uk/a/media/w1024h768/4080db795cff4945a4a1d215d9146d07.jpg",
                "https://m.atcdn.co.uk/a/media/w1024h768/b660f5ba789447be9b867f904c60296b.jpg",
                "https://m.atcdn.co.uk/a/media/w1024h768/f705a641d88f4a6faebecfe7732ad5d0.jpg",
                "https://m.atcdn.co.uk/a/media/w1024h768/dd1b4198474a4dc69c38d368c374798b.jpg"
            };
            var model3Images = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/e08b698f-c0a5-4206-a858-2139c2bbeb84_36db9bfe-9ca6-4f45-b0c8-c8e0f32f6aff.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e08b698f-c0a5-4206-a858-2139c2bbeb84_ab76ab52-257e-4c6e-af57-40e031b17124.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e08b698f-c0a5-4206-a858-2139c2bbeb84_4a823c62-69cc-4d16-b18f-da23e9214be3.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e08b698f-c0a5-4206-a858-2139c2bbeb84_9b890694-51cf-46e4-9ca9-858d1a793744.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/e08b698f-c0a5-4206-a858-2139c2bbeb84_6f7871f3-aff8-4b1e-a0f8-be5ed166bc4b.jpg/1080x810.webp"
            };
            var nineElevenImages = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/b6f62328-e1b9-4b10-a684-20593c9b3a09_1b896b3e-a774-4907-a101-b5b914434379.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/b6f62328-e1b9-4b10-a684-20593c9b3a09_ca70b2d7-a088-4d1c-b2c8-53465f250597.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/b6f62328-e1b9-4b10-a684-20593c9b3a09_99b08ec2-5de2-402a-9596-dfb61d5584f0.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/b6f62328-e1b9-4b10-a684-20593c9b3a09_14d324bf-0066-4756-a6f0-de5fb3a09b14.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/b6f62328-e1b9-4b10-a684-20593c9b3a09_b6a49a27-1a73-4543-a0e1-dfae11c537d9.jpg/1080x810.webp"
            };
            var fPaceImages = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/d42617bc-b094-4d11-ba19-446488350b16_7d66e11f-7cf3-4967-8851-85d5e2c11b1a.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/d42617bc-b094-4d11-ba19-446488350b16_eee5f763-b777-4b24-8d5d-9581f421d926.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/d42617bc-b094-4d11-ba19-446488350b16_44a7fdb5-42b3-4319-ad5e-d3c9e099a108.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/d42617bc-b094-4d11-ba19-446488350b16_ac7f2f37-38ca-45d2-99bc-34bbd9a79dbe.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/d42617bc-b094-4d11-ba19-446488350b16_8c828aa1-37b9-41e3-a637-4d14399ca85e.jpg/1080x810.webp"
            };

            var mx5Images = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/587134cd-6624-4fd9-9e13-0eef92a2ff58_a10f342b-44f2-4738-bf1c-8ad2285d85be.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/587134cd-6624-4fd9-9e13-0eef92a2ff58_e7edf91d-fadb-4f3e-bd21-8556e9622bdb.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/587134cd-6624-4fd9-9e13-0eef92a2ff58_7e4dc249-3f4e-4853-9b72-b467b4953b52.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/587134cd-6624-4fd9-9e13-0eef92a2ff58_936e1c0d-e147-4ae8-b3b3-fd5069692f20.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/587134cd-6624-4fd9-9e13-0eef92a2ff58_5c5dc035-f23b-4dcd-80be-3caa478ad05c.jpg/1080x810.webp"
            };

            var s60Images = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/9df4fe82-ba8a-4bef-b802-5004b610d2ae_debc4a09-70d1-4639-a00e-c0b86bed8086.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/9df4fe82-ba8a-4bef-b802-5004b610d2ae_7104c85c-9fd8-43b4-bae9-979d57bef07d.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/9df4fe82-ba8a-4bef-b802-5004b610d2ae_0e29f609-3141-4fce-a824-443cbf6212b3.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/9df4fe82-ba8a-4bef-b802-5004b610d2ae_b6520b0f-66ff-426a-ba18-d788993560cf.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/9df4fe82-ba8a-4bef-b802-5004b610d2ae_cc42e16d-a4d4-43c4-ba6f-702be581ad65.jpg/1080x810.webp"
            };
            var fiat500Images = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/66876487-73bc-41c3-b72a-3be7ccc9a466_de243160-52f7-4e45-a81d-2e8801dd49aa.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/66876487-73bc-41c3-b72a-3be7ccc9a466_77c5cb07-a4c6-4bdb-9032-4b4a4669c61f.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/66876487-73bc-41c3-b72a-3be7ccc9a466_67c920dd-c1ac-4c98-8f3d-b98f9ccd6364.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/66876487-73bc-41c3-b72a-3be7ccc9a466_086210d5-38da-436b-8b1d-a0bdd2b64aa9.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/66876487-73bc-41c3-b72a-3be7ccc9a466_fcab4b94-ec7c-4219-97fd-b19fcffaba2c.jpg/1080x810.webp"
            };

            var f150Images = new List<string>
            {
                "https://prod.pictures.autoscout24.net/listing-images/90c8dc26-c946-4d80-8953-0902ca9a3c5a_a10539d5-dcaa-4984-899c-5999ad12eba2.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/90c8dc26-c946-4d80-8953-0902ca9a3c5a_a10539d5-dcaa-4984-899c-5999ad12eba2.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/90c8dc26-c946-4d80-8953-0902ca9a3c5a_ecec68d5-3458-4f09-b6e4-972c48e49e8c.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/90c8dc26-c946-4d80-8953-0902ca9a3c5a_c5d5fef2-d7e3-4fe8-b959-c757e059b270.jpg/1080x810.webp",
                "https://prod.pictures.autoscout24.net/listing-images/90c8dc26-c946-4d80-8953-0902ca9a3c5a_db581070-b775-4b2b-b6dd-294cb4f6aecd.jpg/1080x810.webp"
            };

            return model switch
            {
                "A5" => a5Images,
                "Jetta" => jettaImages,
                "F-150" => f150Images,
                "Camaro" => camaroImages,
                "Civic" => civicImages,
                "Altima" => altimaImages,
                "X5" => x5Images,
                "C-Class" => cClassImages,
                "Q5" => q5Images,
                "Elantra" => elantraImages,
                "Soul" => soulImages,
                "Outback" => subaruImages,
                "Model 3" => model3Images,
                "911" => nineElevenImages,
                "F-PACE" => fPaceImages,
                "MX-5" => mx5Images,
                "S60" => s60Images,
                "500" => fiat500Images,
                "Wrangler" => wranglerImages,
                _ => new List<string>()
            };
        }
    }

    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context =
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                ?? throw new InvalidOperationException();

            context.Database.EnsureCreated();

            string[] uniqueUserIds =
            [
                "03f4e858-c059-4e7e-879d-13f407efecbd",
                "0584a5d9-4abe-4989-b43b-8a25bc23babf",
                "1047c3b0-6e3b-4285-ba73-543f76d635da",
                "13e4469e-5e62-44bf-8d82-a092e42ebe64",
                "1cb0fee3-f5f4-407e-b331-6efad37783b0",
                "291fcab9-e9c3-4fd2-934b-53a534b4e3b3",
                "3e44d8f7-6d24-4cc4-bd60-8b8ab2d570dc",
                "40a5a25a-b702-443c-86fd-9457502bd919",
                "5df0e570-4794-4089-b10f-e8b961d3b909",
                "62e3f8ec-a287-46e6-b04d-e91e39ea48f2",
                "8672d5ad-9273-4083-9e77-233bb72a87c7",
                "8a847afe-d28b-4d8b-b7ca-71024f9a6529",
                "8c6372a3-8207-4ca3-a456-b3f11c333c1d",
                "8f9e2da1-4741-4fd3-a5ae-cfa4cf8cdccc",
                "9b46d5a3-be6e-421c-906a-ec4f19b76fd2",
                "9c91852a-5a7d-4d1d-8d47-9d47df9102d3",
                "a5bfc388-9936-4deb-88cc-ee5557f53130",
                "acc19d17-2f4a-434d-9906-53a31a52268e",
                "c856af4a-3ba3-48b4-839f-4cbf35657fc7",
                "e2b519e6-c08d-49e4-af04-070e5762cd53"
            ];

            List<Car> carList =  [ ];

            context.Cars.AddRange(CarSeeder.SeedCars(uniqueUserIds));

            context.SaveChanges();
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var roleManager = serviceScope
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string[] dummyUsernames =
            {
                "john_doe",
                "alice_smith",
                "robert_jackson",
                "emily_wilson",
                "david_martin",
                "olivia_miller",
                "michael_taylor",
                "sophia_jones",
                "william_hall",
                "emma_brown",
                "james_davis",
                "ava_martinez",
                "charles_anderson",
                "mia_morris",
                "benjamin_rodriguez",
                "isabella_nelson",
                "ethan_white",
                "amelia_thomas",
                "samuel_clark",
                "olivia_hill",
                "alexander_lewis",
                "grace_adams",
                "ryan_cook",
                "sophie_carter",
                "nathan_ward",
                "chloe_richardson",
                "daniel_hall",
                "harper_kelly",
                "matthew_russell"
            };

            string[] dummyEmails =
            {
                "john_doe@yahoo.com",
                "alice_smith@gmail.com",
                "robert_jackson@hotmail.com",
                "emily_wilson@outlook.com",
                "david_martin@yahoo.com",
                "olivia_miller@gmail.com",
                "michael_taylor@hotmail.com",
                "sophia_jones@yahoo.com",
                "william_hall@gmail.com",
                "emma_brown@outlook.com",
                "james_davis@yahoo.com",
                "ava_martinez@gmail.com",
                "charles_anderson@hotmail.com",
                "mia_morris@yahoo.com",
                "benjamin_rodriguez@gmail.com",
                "isabella_nelson@outlook.com",
                "ethan_white@yahoo.com",
                "amelia_thomas@gmail.com",
                "samuel_clark@hotmail.com",
                "olivia_hill@yahoo.com",
                "alexander_lewis@gmail.com",
                "grace_adams@hotmail.com",
                "ryan_cook@yahoo.com",
                "sophie_carter@gmail.com",
                "nathan_ward@hotmail.com",
                "chloe_richardson@yahoo.com",
                "daniel_hall@gmail.com",
                "harper_kelly@hotmail.com",
                "matthew_russell@yahoo.com"
            };

            string[] dummyPasswords =
            {
                "P@ssw0rd123",
                "SecurePwd987",
                "Dav!dMartin456",
                "Em1ly_W!lson",
                "Doe$1234",
                "OliviaMiller@567",
                "Taylor_StrongPwd",
                "SophiaJ0nes!",
                "William_Hall123",
                "EmmaBrownPwd!",
                "JamesD@vis789",
                "AvaMart1nez",
                "CharlesA#321",
                "Mia_Morris123",
                "BenjaminPwd@321",
                "IsabellaNelson987!",
                "Ethan_White123",
                "Amelia.Thomas456",
                "SamuelC!ark",
                "Olivia_H!ll789",
                "AlexanderLewis123",
                "GraceAdamsPwd!",
                "Ryan_Cook789",
                "Sophie_Carter123",
                "NathanWard@456",
                "Chloe.Richardson",
                "Daniel_H@ll789",
                "HarperK3lly",
                "Matthew_Russell987!"
            };

            for (int i = 0; i < dummyUsernames.Length; i++)
            {
                var emails = dummyEmails[i];
                var usernames = dummyUsernames[i];
                var passwords = dummyPasswords[i];

                var appUser = await userManager.FindByEmailAsync(emails);

                var newAppUser = new User
                {
                    UserName = usernames,
                    Email = emails,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(newAppUser, passwords);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
