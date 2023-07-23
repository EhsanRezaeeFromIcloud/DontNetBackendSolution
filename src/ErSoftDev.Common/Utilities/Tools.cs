using System.Data;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.Extensions.DependencyModel;

namespace ErSoftDev.Common.Utilities
{
    public static class Tools
    {
        public static IEnumerable<Assembly> GetAllAssemblies()
        {
            var platform = Environment.OSVersion.Platform.ToString();
            var runtimeAssemblyNames = DependencyContext.Default!.GetRuntimeAssemblyNames(platform);

            var res = new List<Assembly>();

            foreach (var assembly in runtimeAssemblyNames)
            {
                try
                {
                    res.Add(Assembly.Load(assembly.FullName));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return res;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);


            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var type = (prop.PropertyType.IsGenericType &&
                            prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? Nullable.GetUnderlyingType(prop.PropertyType)
                    : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type ?? throw new InvalidOperationException());
            }

            if (items.Count <= 0)
                return dataTable;

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                    values[i] = props[i].GetValue(item, null)!;
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static string ToXml(object input)
        {
            var response = "";
            try
            {
                var stringWriter = new StringWriter();
                var serializer = new XmlSerializer(input.GetType());
                serializer.Serialize(stringWriter, input);
                response = stringWriter.ToString();
            }
            catch
            {
                //Ignore
            }

            return response;
        }

        public static bool CheckEMail(string email)
        {
            bool result;
            try
            {
                var unused = new MailAddress(email);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static Image CovertByteArrayToImage(byte[] bytes)
        {
            return Image.Load(bytes);
        }

        public static async Task SaveImage(Image image, string path)
        {
            await image.SaveAsPngAsync(path);
        }

        public static DirectoryInfo CreateDirectory(string path) => Directory.CreateDirectory(path);

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public static long ConvertStringIpToLong(string ip)
        {
            return BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes().Reverse().ToArray(), 0);
        }

        public static bool IsIpTrust(string ip)
        {
            var trustIPs = new List<string>()
            {
                "81.90.152.162", //اینترنت افرانت شرکت
                "127.0.0.1", //خود سرور بانک            
                "::1", // خود سرور بانک
                "82.99.195.122" //اینترنت پارس آنلاین شرکت
            };

            if (trustIPs.Any(itemIp => itemIp == ip))
                return true;


            string startIpAddr = "192.168.1.1";
            string endIpAddr = "192.168.3.254";
            long ipStart = BitConverter.ToInt32(IPAddress.Parse(startIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            long ipEnd = BitConverter.ToInt32(IPAddress.Parse(endIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            long ipConvert = BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes().Reverse().ToArray(), 0);
            if (ipConvert >= ipStart && ipConvert <= ipEnd)
                return true;

            startIpAddr = "79.175.182.50";
            endIpAddr = "79.175.182.62";
            ipStart = BitConverter.ToInt32(IPAddress.Parse(startIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            ipEnd = BitConverter.ToInt32(IPAddress.Parse(endIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            ipConvert = BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes().Reverse().ToArray(), 0);
            if (ipConvert >= ipStart && ipConvert <= ipEnd)
                return true;

            startIpAddr = "192.168.122.1";
            endIpAddr = "192.168.122.255";
            ipStart = BitConverter.ToInt32(IPAddress.Parse(startIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            ipEnd = BitConverter.ToInt32(IPAddress.Parse(endIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            ipConvert = BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes().Reverse().ToArray(), 0);
            if (ipConvert >= ipStart && ipConvert <= ipEnd)
                return true;


            return false;
        }

        /// <summary>
        /// Set any requested character to middle of string 
        /// </summary>
        /// <param name="value">string that must be changed</param>
        /// <param name="character">character that replaced in string</param>
        /// <param name="characterNotChangeCount">how many character from begin and end of string not change</param>
        /// <returns></returns>
        public static string SetMiddleOfStringWithCharacter(this string value, char character,
            int characterNotChangeCount)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (characterNotChangeCount * 2 > value.Length)
                return value;

            return value.Substring(0, characterNotChangeCount) +
                   new string(character, value.Length - 2 * characterNotChangeCount) +
                   value.Substring(value.Length - characterNotChangeCount);
        }

        public static bool CheckCellPhone(this string cellPhone)
        {
            return new Regex("0?9[0-9]{9}").IsMatch(cellPhone);
        }

    }
}