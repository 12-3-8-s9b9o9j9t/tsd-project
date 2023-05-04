const NAME_KEY : string = "NAME";
const ID_KEY   : string = "ID";
const SESSION_KEY: string = "SESSION";

export function saveName(name: string) : void {
    localStorage.setItem(NAME_KEY, name);
}

export function getName() : string {
  let name: string | null = localStorage.getItem(NAME_KEY);
  if (name == null) {
    return "";
  }
  return name;
}

export function saveID(id: number) : void {
  localStorage.setItem(ID_KEY, String(id));
}

export function getID() : number {
  let id: string | null = localStorage.getItem(ID_KEY);
  if (id == null) {
    return -1;
  }
  return +id;
}

export function saveSessionIdentifier(sessionIdentifier: string): void {
  localStorage.setItem(SESSION_KEY, sessionIdentifier);
}

export function getSessionIdentifier(): string {
  const sessionIdentifier: string | null = localStorage.getItem(SESSION_KEY);

  if (sessionIdentifier == null) {
    return "";
  }

  return sessionIdentifier;
}
