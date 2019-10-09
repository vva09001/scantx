import { getDefaultPath } from '../../helpers/urlSync';

const options = [
  {
    label: 'Start',
    key: '',
  },
  {
    label: 'User',
    key: 'user',
  },
  {
    label: 'Company',
    key: 'company',
  },
  {
    label: 'Scan Data',
    key: 'scan-data',
  }
];
const getBreadcrumbOption = () => {
  const preKeys = getDefaultPath();
  let parent, activeChildren;
  options.forEach(option => {
    if (preKeys[option.key]) {
      parent = option;
      (option.children || []).forEach(child => {
        if (preKeys[child.key]) {
          activeChildren = child;
        }
      });
    }
  });
  return { parent, activeChildren };
};
export default options;
export { getBreadcrumbOption };
