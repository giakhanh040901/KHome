import { EpicBoundTemplatePage } from './app.po';

describe('EpicBound App', function() {
  let page: EpicBoundTemplatePage;

  beforeEach(() => {
    page = new EpicBoundTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
