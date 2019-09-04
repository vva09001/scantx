import React from 'react';
import { connect } from 'react-redux';
import {
  TextField
} from '@material-ui/core';
import Dialog, {
  DialogActions,
  DialogContent,
  DialogTitle,
} from 'components/uielements/dialogs';
import Button from 'components/uielements/button';
import { CircularProgress } from 'components/uielements/progress';

class Form extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      params: {}
    }
  }
  componentDidMount() {
  }
  onClose = () => {
    this.props.onToggle(false)
  }
  onSubmit = () => {
    this.setState({
      loading: true
    }, () => {
      this.props.onSubmit(this.state.params, this.onSuccess, this.onSuccess)
    })
  }
  onSuccess = () => {
    this.setState({
      loading: false
    })
  }
  onChange = e => {
    const key = e.target.name;
    const value = e.target.value;
    this.setState({
      params: {
        ...this.state.params,
        [key]: value
      }
    })
  }
  render() {
    const { status } = this.props;
    return (
      <Dialog open={status} onClose={this.onClose}>
          <DialogTitle>{"Create a new ScanTX company"}</DialogTitle>
          <DialogContent>
            <div>
              <TextField
                required
                id="username-input"
                label="Name"
                margin="normal"
                fullWidth
                name="Name"
                onChange={e => this.onChange(e)}
              />
            </div>
            <div>
              <TextField
                id="given-name-input"
                label="Address"
                margin="normal"
                fullWidth
                name="Address"
                onChange={e => this.onChange(e)}
              />
            </div>
            <div>
              <TextField
                id="family-name-input"
                label="Status"
                margin="normal"
                fullWidth
                name="Status"
                onChange={e => this.onChange(e)}
              />
            </div>
          </DialogContent>
          {
            this.state.loading ? (
              <CircularProgress />
            ) : (
              <DialogActions>
                <Button onClick={() => this.onClose()} color="primary">
                  Cancel
                </Button>
                <Button onClick={() => this.onSubmit()} color="primary" autoFocus>
                  Submit
                </Button>
              </DialogActions>
              )
          }
        </Dialog>
    )
  }
}

const mapSateToProps = state => {
  return {
  };
};

const mapDispatchToProps = {
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(Form);