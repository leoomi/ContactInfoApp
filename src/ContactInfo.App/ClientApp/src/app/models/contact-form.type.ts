import { FormControl, FormGroup } from "@angular/forms";
import ContactType from "./contact-type";

type ContactForm = FormGroup<{
    id: FormControl<number | null>,
    personId: FormControl<number | null>,
    info: FormControl<string | null>,
    type: FormControl<ContactType | null>
}>;

export default ContactForm;