import ContactType from "./contact-type";

export default class Contact {
    public id?: number;
    public personId?: number;
    public type?: ContactType;
    public info?: string;
}