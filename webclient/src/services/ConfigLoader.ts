import IConfig from "@/model/Config";

let globalConfig: IConfig;

export async function loadConfig() {
  try {
    const response = await fetch("/config.json");
    globalConfig = await response.json();
  } catch (error) {
    console.error("Error loading config:", error);
  }
}

export function getConfig() {
  return globalConfig;
}
