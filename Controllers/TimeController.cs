using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace TimezoneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeController : ControllerBase
    {
        [HttpGet("local")]
        public IActionResult GetLocalTime()
        {
            var localTime = DateTimeOffset.Now;
            var timeZone = TimeZoneInfo.Local;

            return Ok(new
            {
                time = localTime.ToString("HH:mm:ss"),
                timezone = timeZone.Id
            });
        }

        [HttpGet("timezone")]
        public IActionResult GetTimeByTimezone([FromQuery] string tz)
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(tz);
                var utcNow = DateTimeOffset.UtcNow;
                var tzTime = TimeZoneInfo.ConvertTime(utcNow, timeZone);

                return Ok(new
                {
                    time = tzTime.ToString("HH:mm:ss"),
                    timezone = timeZone.Id
                });
            }
            catch (TimeZoneNotFoundException)
            {
                return BadRequest($"Timezone '{tz}' not found.");
            }
            catch (InvalidTimeZoneException)
            {
                return BadRequest($"Timezone '{tz}' is invalid.");
            }
        }

        [HttpGet("all-timezones")]
        public IActionResult GetAllTimezones()
        {
            var timezones = TimeZoneInfo.GetSystemTimeZones();
            var list = new List<string>();

            foreach (var tz in timezones)
            {
                list.Add(tz.Id);
            }

            return Ok(list);
        }
    }
}

