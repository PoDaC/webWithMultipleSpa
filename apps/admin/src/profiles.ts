import { autoinject } from 'aurelia-framework';

@autoinject
export class Profiles {
    profile = "";
    activate() {
        this.profile = "";
    }

}