const NAME_KEY : string = "NAME";
const ID_KEY   : string = "ID";
const SESSION_KEY: string = "SESSION";
const OWNER: string = "OWNER";

export function saveName(name: string) : void {
    sessionStorage.setItem(NAME_KEY, name);
}

export function getName() : string {
  let name: string | null = sessionStorage.getItem(NAME_KEY);
  if (name == null) {
    return "";
  }
  return name;
}

export function saveID(id: number) : void {
  sessionStorage.setItem(ID_KEY, String(id));
}

export function getID() : number {
  let id: string | null = sessionStorage.getItem(ID_KEY);
  if (id == null) {
    return -1;
  }
  return +id;
}

export function saveSessionIdentifier(sessionIdentifier: string): void {
  sessionStorage.setItem(SESSION_KEY, sessionIdentifier);
}

export function getSessionIdentifier(): string {
  const sessionIdentifier: string | null = sessionStorage.getItem(SESSION_KEY);

  if (sessionIdentifier == null) {
    return "";
  }

  return sessionIdentifier;
}

export function setOwner(): void {
  sessionStorage.setItem(OWNER, String(getID()));
}

export function getOwner(): number {
  const owner: string | null = sessionStorage.getItem(OWNER);
  if (owner == null) {
    return -1;
  }
  return +owner;
}

export function isOwner(): boolean {
  return getID() == getOwner()
}
