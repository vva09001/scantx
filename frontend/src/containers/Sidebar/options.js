import { getDefaultPath } from '../../helpers/urlSync';

const options = [
  {
    label: 'Start',
    key: 'start',
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
    label: 'Divices',
    key: 'divices',
  },
  {
    label: 'Scan Data',
    key: 'scan-data',
  },
  {
    label: 'Setup',
    key: 'setup',
  },
  {
    label: 'About',
    key: 'about',
  },
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
