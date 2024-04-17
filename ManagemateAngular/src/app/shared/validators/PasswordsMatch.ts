import { AbstractControl } from "@angular/forms"

export const PasswordsMatch = (passwordName: string, confirmPasswordName: string) => {

    const validator = (form: AbstractControl) => {
        const passwordControl = form.get(passwordName);
        const confirmPasswordControl = form.get(confirmPasswordName);

        if (!passwordControl || !confirmPasswordControl) return;

        if (passwordControl.value !== confirmPasswordControl.value) {
            confirmPasswordControl.setErrors({ notMatch: true });
        } else {
            const errors = confirmPasswordControl.errors;
            if (!errors) return;

            delete errors.notMatch;
            confirmPasswordControl.setErrors(errors);
        }
    }
    return validator;
}