import { Validators } from "@angular/forms";
import ContactForm from "../models/contact-form.type";
import ContactType from "../models/contact-type";

export default class CustomValidators
{
  validateContact(form: ContactForm) {
    if (form.get('type')?.value === ContactType.Email) {
      return Validators.email.apply();
    }
  }
}