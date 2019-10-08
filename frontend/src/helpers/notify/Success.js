import Noty from "noty";

const notySuccess = mess => {
  new Noty({
    type: "success",
    text: mess,
    layout: "topRight",
    timeout: 3000
  }).show();
};

export default notySuccess;
