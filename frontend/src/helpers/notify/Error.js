import Noty from "noty";

const notyError = mess => {
  return new Noty({
    type: "error",
    text: mess,
    layout: "topRight",
    timeout: 3000
  }).show();
};

export default notyError;
