import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";
import customParseFormat from "dayjs/plugin/customParseFormat";
import { TimeType } from "@/enum/TimeType";

// Load the plugins
dayjs.extend(utc);
dayjs.extend(customParseFormat);

export default class DateTimeHelper {
  static getUtcDateTimeNow(timeType: TimeType): string {
    const nowUTC = dayjs.utc();

    switch (timeType) {
      case TimeType.CurrentStandart:
        return nowUTC.format("DD-MM-YYYY HH:mm:ss");
      case TimeType.FileNameDateTime:
        return nowUTC.format("DDMMYYYY_HHmmss");
    }
  }

  static formatDateToLocalString(dateTime: Date): string {
    return dayjs(dateTime).format("DD-MM-YYYY HH:mm:ss");
  }

  static convertStringToIso(input: string): Date {
    const convertedData = dayjs.utc(input, "DD-MM-YYYY HH:mm:ss");
    const output = convertedData.format("YYYY-MM-DDTHH:mm:ss.SSSZ");
    return new Date(output);
  }
}
