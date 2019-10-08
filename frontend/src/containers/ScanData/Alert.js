import React from "react";
import Dialog, {
  DialogActions,
  DialogContent,
  DialogTitle,
  DialogContentText
} from "components/uielements/dialogs";
import Button from "components/uielements/button";
// import { CircularProgress } from "components/uielements/progress";

class Alert extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false
    };
  }

  render() {
    const { status, onSubmit, onClose } = this.props;
    return (
      <Dialog open={status} onClose={() => onClose(false)}>
        <DialogTitle>{"Remove scan data"}</DialogTitle>
        <DialogContent>
          <DialogContentText>Are you sure to remove ?</DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => onClose(false)} color="primary">
            Cancel
          </Button>
          <Button onClick={onSubmit} color="primary" autoFocus>
            Submit
          </Button>
        </DialogActions>
      </Dialog>
    );
  }
}

export default Alert;
