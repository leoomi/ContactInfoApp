import Contact from "./contact";

export default class Person {
    public id?: number;
    public userId?: number;
    public firstName?: string;
    public lastName?: string;
    public contacts?: Contact[];
}