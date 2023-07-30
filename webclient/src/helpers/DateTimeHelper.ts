import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";
import { TimeType } from "@/enum/TimeType";

export default class DateTimeHelper {
  static getUtcDateTimeNow(timeType: TimeType) {
    const nowUTC = dayjs.utc();

    switch (timeType) {
      case TimeType.CurrentStandart:
        return nowUTC.format("DD-MM-YYYY HH:mm:ss");
      case TimeType.FileNameDateTime:
        return nowUTC.format("DDMMYYYY_HHmmss");
    }
  }

  static formatDateToLocalString(dateTime: Date) {
    return dayjs(dateTime).format("DD-MM-YYYY HH:mm:ss");
  }
}
