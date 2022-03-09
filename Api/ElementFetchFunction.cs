﻿namespace DotDev.Api
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Dapper;
    using Dotdev.Core.Element;

    public static class ElementFetchFunction
    {
        [FunctionName("ElementFetch")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("ElementFetch!");
            var connStr = Environment.GetEnvironmentVariable("dotdev_cs");
            var query = "select * from view_Elements";

            var fetched = await FetchAsync<ElementInfo>(connStr, query, log);
            log.LogInformation("Returned from DB for ElementFetch {retrieved}", fetched?.Count());
            var result = fetched ?? MakeDefaults() ?? new List<ElementInfo>();

            return new OkObjectResult(result);
        }

        private static async Task<IEnumerable<T>> FetchAsync<T>(string connStr, string query, ILogger log)
        {
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    log.LogInformation("connected to DB for ElementFetch!");
                    return await conn.QueryAsync<T>(query, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "ElementFetch DB fail");
                throw;
            }
        }

        private static IEnumerable<ElementInfo> MakeDefaults()
        {
            var result = new List<ElementInfo>();
            //string[] categories = ["alkali-metals", "alkaline-earth-metals", "lanthanoids", "actinoids", "transition-metals", "post-transition-metals", "metalloids", "other-nonmetals", "noble-gasses", "unknown"];

            result.Add(new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1));
            result.Add(new ElementInfo(201, "Mi", "Mithril", "?", "[1]", "transition-metal", 9, 1));
            result.Add(new ElementInfo(2, "He", "Helium", 4.0026, "[2]", "noble-gas", 18, 1));
            result.Add(new ElementInfo(3, "Li", "Lithium", 6.94, "[3]", "alkali-metal", 1, 2));
            result.Add(new ElementInfo(4, "Be", "Beryllium", 9.0122, "[2, 2]", "alkaline-earth-metal", 2, 2));
            result.Add(new ElementInfo(5, "B", "Boron", 10.81, "[2, 3]", "metalloid", 13, 2));
            result.Add(new ElementInfo(6, "C", "Carbon", 12.011, "[2, 4]", "other-nonmetal", 14, 2));
            result.Add(new ElementInfo(7, "N", "Nitrogen", 14.007, "[2, 5]", "other-nonmetal", 15, 2));
            result.Add(new ElementInfo(8, "O", "Oxygen", 15.999, "[2, 6]", "other-nonmetal", 16, 2));
            result.Add(new ElementInfo(9, "F", "Flourine", 18.998, "[2, 7]", "other-nonmetal", 17, 2));
            result.Add(new ElementInfo(10, "Ne", "Neon", 20.180, "[2, 8]", "noble-gas", 18, 2));
            result.Add(new ElementInfo(11, "Na", "Sodium", 22.990, "[2, 8, 1]", "alkali-metal", 1, 3));
            result.Add(new ElementInfo(12, "Mg", "Magnesium", 24.305, "[2, 8, 2]", "alkaline-earth-metal", 2, 3));
            result.Add(new ElementInfo(13, "Al", "Aluminium", 26.982, "[2, 8, 3]", "post-transition-metal", 13, 3));
            result.Add(new ElementInfo(14, "Si", "Silicon", 28.085, "[2, 8, 4]", "metalloid", 14, 3));
            result.Add(new ElementInfo(15, "P", "Phosphorus", 30.974, "[2, 8, 5]", "other-nonmetal", 15, 3));
            result.Add(new ElementInfo(16, "S", "Sulfur", 32.06, "[2, 8, 6]", "other-nonmetal", 16, 3));
            result.Add(new ElementInfo(17, "Cl", "Chlorine", 35.45, "[2, 8, 7]", "other-nonmetal", 17, 3));
            result.Add(new ElementInfo(18, "Ar", "Argon", 39.948, "[2, 8, 8]", "noble-gas", 18, 3));
            result.Add(new ElementInfo(19, "K", "Potassium", 39.098, "[2, 8, 8, 1]", "alkali-metal", 1, 4));
            result.Add(new ElementInfo(20, "Ca", "Calcium", 40.078, "[2, 8, 8, 2]", "alkaline-earth-metal", 2, 4));
            result.Add(new ElementInfo(21, "Sc", "Scandium", 44.956, "[2, 8, 9, 2]", "transition-metal", 3, 4));
            result.Add(new ElementInfo(22, "Ti", "Titanium", 47.867, "[2, 8, 10, 2]", "transition-metal", 4, 4));
            result.Add(new ElementInfo(23, "V", "Vanadium", 50.942, "[2, 8, 11, 2]", "transition-metal", 5, 4));
            result.Add(new ElementInfo(24, "Cr", "Chromium", 51.996, "[2, 8, 13, 1]", "transition-metal", 6, 4));
            result.Add(new ElementInfo(25, "Mn", "Manganese", 54.938, "[2, 8, 13, 2]", "transition-metal", 7, 4));
            result.Add(new ElementInfo(26, "Fe", "Iron", 55.845, "[2, 8, 14, 2]", "transition-metal", 8, 4));
            result.Add(new ElementInfo(27, "Co", "Cobalt", 58.933, "[2, 8, 15, 2]", "transition-metal", 9, 4));
            result.Add(new ElementInfo(28, "Ni", "Nickel", 58.693, "[2, 8, 16, 2]", "transition-metal", 10, 4));
            result.Add(new ElementInfo(29, "Cu", "Copper", 63.546, "[2, 8, 18, 1]", "transition-metal", 11, 4));
            result.Add(new ElementInfo(30, "Zn", "Zinc", 65.38, "[2, 8, 18, 2]", "transition-metal", 12, 4));
            result.Add(new ElementInfo(31, "Ga", "Gallium", 69.723, "[2, 8, 18, 3]", "post-transition-metal", 13, 4));
            result.Add(new ElementInfo(32, "Ge", "Germanium", 72.630, "[2, 8, 18, 4]", "metalloid", 14, 4));
            result.Add(new ElementInfo(33, "As", "Arsenic", 74.922, "[2, 8, 18, 5]", "metalloid", 15, 4));
            result.Add(new ElementInfo(34, "Se", "Selenium", 78.971, "[2, 8, 18, 6]", "other-nonmetal", 16, 4));
            result.Add(new ElementInfo(35, "Br", "Bromine", 79.904, "[2, 8, 18, 7]", "other-nonmetal", 17, 4));
            result.Add(new ElementInfo(36, "Kr", "Krypton", 83.798, "[2, 8, 18, 8]", "noble-gas", 18, 4));
            result.Add(new ElementInfo(37, "Rb", "Rubidium", 85.468, "[2, 8, 18, 8, 1]", "alkali-metal", 1, 5));
            result.Add(new ElementInfo(38, "Sr", "Strontium", 87.62, "[2, 8, 18, 8, 2]", "alkaline-earth-metal", 2, 5));
            result.Add(new ElementInfo(39, "Y", "Yttrium", 88.906, "[2, 8, 18, 9, 2]", "transition-metal", 3, 5));
            result.Add(new ElementInfo(40, "Zr", "Zirconium", 91.224, "[2, 8, 18, 10, 2]", "transition-metal", 4, 5));
            result.Add(new ElementInfo(41, "Nb", "Niobium", 92.906, "[2, 8, 18, 12, 1]", "transition-metal", 5, 5));
            result.Add(new ElementInfo(42, "Mo", "Molybdenum", 95.95, "[2, 8, 18, 13, 1]", "transition-metal", 6, 5));
            result.Add(new ElementInfo(43, "Tc", "Technetium", "(98)", "[2, 8, 18, 13, 2]", "transition-metal", 7, 5));
            result.Add(new ElementInfo(44, "Ru", "Ruthenium", 101.07, "[2, 8, 18, 15, 1]", "transition-metal", 8, 5));
            result.Add(new ElementInfo(45, "Rh", "Rhodium", 102.91, "[2, 8, 18, 16, 1]", "transition-metal", 9, 5));
            result.Add(new ElementInfo(46, "Pd", "Palladium", 106.42, "[2, 8, 18, 18]", "transition-metal", 10, 5));
            result.Add(new ElementInfo(47, "Ag", "Silver", 107.87, "[2, 8, 18, 18, 1]", "transition-metal", 11, 5));
            result.Add(new ElementInfo(48, "Cd", "Cadmium", 112.41, "[2, 8, 18, 18, 2]", "transition-metal", 12, 5));
            result.Add(new ElementInfo(49, "In", "Indium", 114.82, "[2, 8, 18, 18, 3]", "post-transition-metal", 13, 5));
            result.Add(new ElementInfo(50, "Sn", "Tin", 204.38, "[2, 8, 18, 18, 4]", "post-transition-metal", 14, 5));
            result.Add(new ElementInfo(51, "Sb", "Antimony", 121.76, "[2, 8, 18, 18, 5]", "metalloid", 15, 5));
            result.Add(new ElementInfo(52, "Te", "Tellurium", 127.60, "[2, 8, 18, 18, 6]", "metalloid", 16, 5));
            result.Add(new ElementInfo(53, "I", "Iodine", 126.90, "[2, 8, 18, 18, 7]", "other-nonmetal", 17, 5));
            result.Add(new ElementInfo(54, "Xe", "Xenon", 131.29, "[2, 8, 18, 18, 8]", "noble-gas", 18, 5));
            result.Add(new ElementInfo(55, "Cs", "Caesium", 132.91, "[2, 8, 18, 18, 8, 1]", "alkali-metal", 1, 6));
            result.Add(new ElementInfo(56, "Ba", "Barium", 137.33, "[2, 8, 18, 18, 8, 2]", "alkaline-earth-metal", 2, 6));
            //result.Add(new ElementInfo(57, "La", "Lanthanum", 138.91, "[2, 8, 18, 18, 9, 2]", "lanthanoid", 4, 9));
            //result.Add(new ElementInfo(58, "Ce", "Cerium", 140.12, "[2, 8, 18, 19, 9, 2]", "lanthanoid", 5, 9));
            //result.Add(new ElementInfo(59, "Pr", "Praseodymium", 140.91, "[2, 8, 18, 21, 8, 2]", "lanthanoid", 6, 9));
            //result.Add(new ElementInfo(60, "Nd", "Neodymium", 144.24, "[2, 8, 18, 22, 8, 2]", "lanthanoid", 7, 9));
            //result.Add(new ElementInfo(61, "Pm", "Promethium", 144.24, "[2, 8, 18, 23, 8, 2]", "lanthanoid", 8, 9));
            //result.Add(new ElementInfo(62, "Sm", "Samarium", 150.36, "[2, 8, 18, 24, 8, 2]", "lanthanoid", 9, 9));
            //result.Add(new ElementInfo(63, "Eu", "Europium", 151.96, "[2, 8, 18, 25, 8, 2]", "lanthanoid", 10, 9));
            //result.Add(new ElementInfo(64, "Gd", "Gadolinium", 157.25, "[2, 8, 18, 25, 9, 2]", "lanthanoid", 11, 9));
            //result.Add(new ElementInfo(65, "Tb", "Terbium", 158.93, "[2, 8, 18, 27, 8, 2]", "lanthanoid", 12, 9));
            //result.Add(new ElementInfo(66, "Dy", "Dysprosium", 162.50, "[2, 8, 18, 28, 8, 2]", "lanthanoid", 13, 9));
            //result.Add(new ElementInfo(67, "Ho", "Holmium", 164.93, "[2, 8, 18, 29, 8, 2]", "lanthanoid", 14, 9));
            //result.Add(new ElementInfo(68, "Er", "Erbium", 167.26, "[2, 8, 18, 30, 8, 2]", "lanthanoid", 15, 9));
            //result.Add(new ElementInfo(69, "Tm", "Thulium", 168.93, "[2, 8, 18, 31, 8, 2]", "lanthanoid", 16, 9));
            //result.Add(new ElementInfo(70, "Yb", "Ytterbium", 173.05, "[2, 8, 18, 32, 8, 2]", "lanthanoid", 17, 9));
            //result.Add(new ElementInfo(71, "Lu", "Lutetium", 174.97, "[2, 8, 18, 32, 9, 2]", "lanthanoid", 18, 9));
            result.Add(new ElementInfo(72, "Hf", "Hafnium", 178.49, "[2, 8, 18, 32, 10, 2]", "transition-metal", 4, 6));
            result.Add(new ElementInfo(73, "Ta", "Tantalum", 180.95, "[2, 8, 18, 32, 11, 2]", "transition-metal", 5, 6));
            result.Add(new ElementInfo(74, "W", "Tungsten", 183.84, "[2, 8, 18, 32, 12, 2]", "transition-metal", 6, 6));
            result.Add(new ElementInfo(75, "Re", "Rhenium", 186.21, "[2, 8, 18, 32, 13, 2]", "transition-metal", 7, 6));
            result.Add(new ElementInfo(76, "Os", "Osmium", 190.23, "[2, 8, 18, 32, 14, 2]", "transition-metal", 8, 6));
            result.Add(new ElementInfo(77, "Ir", "Iridium", 192.22, "[2, 8, 18, 32, 15, 2]", "transition-metal", 9, 6));
            result.Add(new ElementInfo(78, "Pt", "Platinum", 195.08, "[2, 8, 18, 32, 17, 1]", "transition-metal", 10, 6));
            result.Add(new ElementInfo(79, "Au", "Gold", 196.97, "[2, 8, 18, 32, 18, 1]", "transition-metal", 11, 6));
            result.Add(new ElementInfo(80, "Hg", "Mercury", 200.59, "[2, 8, 18, 32, 18, 2]", "transition-metal", 12, 6));
            result.Add(new ElementInfo(81, "Tl", "Thallium", 204.38, "[2, 8, 18, 32, 18, 3]", "post-transition-metal", 13, 6));
            result.Add(new ElementInfo(82, "Pb", "Lead", 207.2, "[2, 8, 18, 32, 18, 4]", "post-transition-metal", 14, 6));
            result.Add(new ElementInfo(83, "Bi", "Bismuth", 208.98, "[2, 8, 18, 32, 18, 5]", "post-transition-metal", 15, 6));
            result.Add(new ElementInfo(84, "Po", "Polonium", "(209)", "[2, 8, 18, 32, 18, 6]", "post-transition-metal", 16, 6));
            result.Add(new ElementInfo(85, "At", "Astatine", "(210)", "[2, 8, 18, 32, 18, 7]", "metalloid", 17, 6));
            result.Add(new ElementInfo(86, "Rn", "Radon", "(222)", "[2, 8, 18, 32, 18, 8]", "noble-gas", 18, 6));
            result.Add(new ElementInfo(87, "Fr", "Francium", "(223)", "[2, 8, 18, 32, 18, 8, 1]", "alkali-metal", 1, 7));
            result.Add(new ElementInfo(88, "Ra", "Radium", "(226)", "[2, 8, 18, 32, 18, 8, 2]", "alkaline-earth-metal", 2, 7));
            //result.Add(new ElementInfo(89, "Ac", "Actinium", "(227)", "[2, 8, 18, 32, 18, 9, 2]", "actinoid", 4, 10));
            //result.Add(new ElementInfo(90, "Th", "Thorium", 232.04, "[2, 8, 18, 32, 18, 10, 2]", "actinoid", 5, 10));
            //result.Add(new ElementInfo(91, "Pa", "Protactinium", 231.04, "[2, 8, 18, 32, 20, 9, 2]", "actinoid", 6, 10));
            //result.Add(new ElementInfo(92, "U", "Uranium", 238.03, "[2, 8, 18, 32, 21, 9, 2]", "actinoid", 7, 10));
            //result.Add(new ElementInfo(93, "Np", "Neptunium", "(237)", "[2, 8, 18, 32, 22, 9, 2]", "actinoid", 8, 10));
            //result.Add(new ElementInfo(94, "Pu", "Plutonium", "(244)", "[2, 8, 18, 32, 24, 8, 2]", "actinoid", 9, 10));
            //result.Add(new ElementInfo(95, "Am", "Americium", "(243)", "[2, 8, 18, 32, 25, 8, 2]", "actinoid", 10, 10));
            //result.Add(new ElementInfo(96, "Cm", "Curium", "(247)", "[2, 8, 18, 32, 25, 9, 2]", "actinoid", 11, 10));
            //result.Add(new ElementInfo(97, "Bk", "Berkelium", "(247)", "[2, 8, 18, 32, 27, 8, 2]", "actinoid", 12, 10));
            //result.Add(new ElementInfo(98, "Cf", "Californium", "(251)", "[2, 8, 18, 32, 28, 8, 2]", "actinoid", 13, 10));
            //result.Add(new ElementInfo(99, "Es", "Einsteinium", "(252)", "[2, 8, 18, 32, 29, 8, 2]", "actinoid", 14, 10));
            //result.Add(new ElementInfo(100, "Fm", "Fermium", "(257)", "[2, 8, 18, 32, 30, 8, 2]", "actinoid", 15, 10));
            //result.Add(new ElementInfo(101, "Md", "Mendelevium", "(258)", "[2, 8, 18, 32, 31, 8, 2]", "actinoid", 16, 10));
            //result.Add(new ElementInfo(102, "No", "Nobelium", "(259)", "[2, 8, 18, 32, 32, 8, 2]", "actinoid", 17, 10));
            //result.Add(new ElementInfo(103, "Lr", "Lawrencium", "(266)", "[2, 8, 18, 32, 32, 8, 3]", "actinoid", 18, 10));
            result.Add(new ElementInfo(104, "Rf", "Rutherfordium", "(267)", "[2, 8, 18, 32, 32, 10, 2]", "transition-metal", 4, 7));
            result.Add(new ElementInfo(105, "Db", "Dubnium", "(268)", "[2, 8, 18, 32, 32, 11, 2]", "transition-metal", 5, 7));
            result.Add(new ElementInfo(106, "Sg", "Seaborgium", "(269)", "[2, 8, 18, 32, 32, 12, 2]", "transition-metal", 6, 7));
            result.Add(new ElementInfo(107, "Bh", "Bohrium", "(270)", "[2, 8, 18, 32, 32, 13, 2]", "transition-metal", 7, 7));
            result.Add(new ElementInfo(108, "Hs", "Hassium", "(277)", "[2, 8, 18, 32, 32, 14, 2]", "transition-metal", 8, 7));
            result.Add(new ElementInfo(109, "Mt", "Meitnerium", "(278)", "[2, 8, 18, 32, 32, 15, 2]", "unknown", 9, 7));
            result.Add(new ElementInfo(110, "Ds", "Darmstadtium", "(281)", "[2, 8, 18, 32, 32, 17, 1]", "unknown", 10, 7));
            result.Add(new ElementInfo(111, "Rg", "Roentgenium", "(282)", "[2, 8, 18, 32, 32, 17, 2]", "unknown", 11, 7));
            result.Add(new ElementInfo(112, "Cn", "Copernicium", "(282)", "[2, 8, 18, 32, 32, 18, 2]", "transition-metal", 12, 7));
            result.Add(new ElementInfo(113, "Nh", "Nihonium", "(286)", "[2, 8, 18, 32, 32, 18, 3]", "unknown", 13, 7));
            result.Add(new ElementInfo(114, "Fl", "Flerovium", "(289)", "[2, 8, 18, 32, 32, 18, 4]", "post-transition-metal", 14, 7));
            result.Add(new ElementInfo(115, "Mc", "Moscovium", "(290)", "[2, 8, 18, 32, 32, 18, 5]", "unknown", 15, 7));
            result.Add(new ElementInfo(116, "Lv", "Livermorium", "(293)", "[2, 8, 18, 32, 32, 18, 6]", "unknown", 16, 7));
            result.Add(new ElementInfo(117, "Ts", "Tennessine", "(294)", "[2, 8, 18, 32, 32, 18, 7]", "unknown", 17, 7));
            result.Add(new ElementInfo(118, "Og", "Oganesson", "(294)", "[2, 8, 18, 32, 32, 18, 8]", "unknown", 18, 7));
            return result;
        }

    }
}
