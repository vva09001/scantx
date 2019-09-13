import React from "react";
import { connect } from "react-redux";
import { TextField } from "@material-ui/core";
import Dialog, {
  DialogActions,
  DialogContent,
  DialogTitle
} from "components/uielements/dialogs";
import Button from "components/uielements/button";
import { CircularProgress } from "components/uielements/progress";

class Form extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      params: {}
    };
  }

  onClose = () => {
    this.props.onToggle(false);
  };

  onSubmit = () => {
    this.setState(
      {
        loading: true
      },
      () => {
        this.props.onSubmit(this.state.params, this.onSuccess, this.onSuccess);
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
        this.onClose();
      }
    );
  };

  onChange = e => {
    const key = e.target.name;
    const value = e.target.value;
    this.setState({
      params: {
        ...this.props.params,
        [key]: value
      }
    }, () => {
      this.props.onChange(this.state.params)
    });
  };

  render() {
    const { status } = this.props;
    return (
      <Dialog open={status} onClose={this.onClose}>
        {this.state.loading ? (
          <CircularProgress />
        ) : (
          <React.Fragment>
            <DialogTitle>{"Create/Edit ScanTX company"}</DialogTitle>
            <DialogContent>
              <div>
                <TextField
                  required
                  id="username-input"
                  label="Name"
                  margin="normal"
                  fullWidth
                  name="name"
                  value={this.props.params.name}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <TextField
                  id="given-name-input"
                  label="Address"
                  margin="normal"
                  fullWidth
                  name="address"
                  value={this.props.params.address}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <TextField
                  id="family-name-input"
                  label="Status"
                  margin="normal"
                  fullWidth
                  name="status"
                  value={this.props.params.status}
                  onChange={e => this.onChange(e)}
                />
              </div>
            </DialogContent>
            <DialogActions>
              <Button onClick={() => this.onClose()} color="primary">
                Cancel
              </Button>
              <Button onClick={() => this.onSubmit()} color="primary" autoFocus>
                Submit
              </Button>
            </DialogActions>
          </React.Fragment>
        )}
      </Dialog>
    );
  }
}

const mapSateToProps = state => {
  return {};
};

const mapDispatchToProps = {};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(Form);
