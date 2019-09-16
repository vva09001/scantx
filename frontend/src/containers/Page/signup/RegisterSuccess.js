import React from "react";
import Dialog, {
  DialogActions,
  DialogContent,
  DialogTitle,
  DialogContentText
} from "components/uielements/dialogs";
import Button from "components/uielements/button";
import { CircularProgress } from "components/uielements/progress";

class RegisterSuccess extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false
    };
  }

  render() {
    const { status, onSubmit } = this.props;
    return (
      <Dialog open={status}>
        <DialogTitle>{"Register Success"}</DialogTitle>
        <DialogContent>
          <DialogContentText>Welcome to ScanTx</DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={onSubmit} color="primary" autoFocus>
            OK
          </Button>
        </DialogActions>
      </Dialog>
    );
  }
}

export default RegisterSuccess;
