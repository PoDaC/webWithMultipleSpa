import {PLATFORM} from 'aurelia-pal';
import {Router, RouterConfiguration} from 'aurelia-router';

export class App {
  public router: Router;

  public configureRouter(config: RouterConfiguration, router: Router): Promise<void> | PromiseLike<void> | void {
    config.title = 'Aurelia';
    config.options.root ="/admin";
    config.options.pushState = true;
    config.map([
      {
        route: ['', 'welcome'],
        name: 'welcome',
        moduleId: PLATFORM.moduleName('./welcome'),
        nav: true,
        title: 'Welcome'
      },
      {
        route: 'users',
        name: 'users',
        moduleId: PLATFORM.moduleName('./users'),
        nav: true,
        title: 'Github Users'
      },
      {
        route: 'child-router',
        name: 'child-router',
        moduleId: PLATFORM.moduleName('./child-router'),
        nav: true,
        title: 'Child Router'
      },
      {
        route: 'profiles',
        name: 'profiles',
        moduleId: PLATFORM.moduleName('./profiles'),
        nav: true,
        title: 'Profiles'
      }
    ]);

    this.router = router;
  }
}
