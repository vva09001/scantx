import React from "react";
import Dialog, {
  DialogActions,
  DialogContent,
  DialogTitle,
  DialogContentText
} from "components/uielements/dialogs";
import Button from "components/uielements/button";
// import { CircularProgress } from "components/uielements/progress";
import { companyActions } from "redux/actions";
import { connect } from "react-redux";

class Alert extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false
    };
  }

  onDelete = () => {
    this.setState(
      {
        loading: true
      },
      () => {
        this.props.delete(this.props.selected, this.onSuccess, this.onFail);
      }
    );
  };

  onSuccess = () => {
    this.setState(
      {
        loading: false
      },
      () => {
        this.props.remove(true);
        this.props.onClose(false);
      }
    );
  };

  onFail = () => {
    this.setState({
      loading: false
    });
  };

  render() {
    const { status, onClose } = this.props;
    return (
      <Dialog open={status} onClose={() => onClose(false)}>
        <React.Fragment>
          <DialogTitle>{"Remove company"}</DialogTitle>
          <DialogContent>
            <DialogContentText>Are you sure to remove ?</DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => onClose(false)} color="primary">
              Cancel
            </Button>
            <Button onClick={() => this.onDelete()} color="primary" autoFocus>
              Submit
            </Button>
          </DialogActions>
        </React.Fragment>
      </Dialog>
    );
  }
}
const mapSateToProps = state => {
  return {};
};

const mapDispatchToProps = {
  delete: companyActions.delete
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(Alert);
